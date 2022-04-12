/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;
using System.Linq;
using System.Collections.Generic;
using DOL.GS;
using DOL.Database;
using DOL.GS.Finance;

namespace DOL.GS.Profession
{
    public class MerchantCatalogEntry
    {
        public byte SlotPosition { get; } = 0;
        public byte Page { get; } = 0;
        public ItemTemplate Item { get; }
        public long CurrencyAmount {get; } = 0;

        public MerchantCatalogEntry(byte slotPosition, byte page, ItemTemplate itemTemplate)
            : this(slotPosition, page, itemTemplate, 0) { }

        public MerchantCatalogEntry(byte slotPosition, byte page, ItemTemplate itemTemplate, long currencyAmount)
        {
            SlotPosition = slotPosition;
            Page = page;
            Item = itemTemplate;
            CurrencyAmount = currencyAmount;
        }
    }

    public class MerchantCatalogPage
    {
        private const byte FIRST_SLOT = 0;
        private const byte MAX_SLOTS = 30;

        private List<MerchantCatalogEntry> entries = new List<MerchantCatalogEntry>();

        public byte Number { get; }
        public Currency Currency { get; private set; } = Money.Copper;
        public ItemTemplate CurrencyItem => Currency is ItemCurrency itemCurrency ? itemCurrency.Item : null;

        public MerchantCatalogPage(byte number)
        {
            Number = number;
        }

        public bool Add(ItemTemplate item, byte toSlot)
        {
            var isSlotInvalid = toSlot < FIRST_SLOT || toSlot > MAX_SLOTS;
            if (isSlotInvalid) return false;
            entries.Add(new MerchantCatalogEntry(toSlot, Number, item));
            return true;
        }

        public bool Add(ItemTemplate item)
        {
            var nextSlot = GetNextFreeSlot();
            return Add(item, nextSlot);
        }

        public bool Remove(byte slot)
        {
            var entry = entries.Where(x => x.SlotPosition == slot);
            if (!entry.Any()) return false;
            return entries.Remove(entry.First());
        }

        public void SetCurrency(Currency currency)
        {
            Currency = currency;
        }

        public IEnumerable<MerchantCatalogEntry> GetAllEntries()
            => entries.ToArray();

        public MerchantCatalogEntry GetEntry(byte slotPosition)
        {
            var entry = entries.Where(x => x.SlotPosition == slotPosition).FirstOrDefault();
            return entry != null ? entry : new MerchantCatalogEntry(255, 255, null);
        }

        public byte GetNextFreeSlot()
        {
            foreach (byte i in Enumerable.Range(FIRST_SLOT, MAX_SLOTS))
            {
                if (entries.Where(x => x.SlotPosition == i).Any() == false)
                {
                    return i;
                }
            }
            byte invalidSlot = 255;
            return invalidSlot;
        }

        public int EntryCount => entries.Count;
    }

    public class MerchantCatalog
    {
        private const int FIRST_PAGE = 0;
        private const int MAX_PAGES = 5;
        private SortedList<byte, MerchantCatalogPage> merchantPages = new SortedList<byte, MerchantCatalogPage>();

        public string ItemListId { get; private set; } = "NotLoadedFromDatabase";

        private MerchantCatalog() { }

        public static MerchantCatalog Create() => new MerchantCatalog();

        public static MerchantCatalog Create(string itemListId) => new MerchantCatalog() { ItemListId = itemListId };

        public static MerchantCatalog LoadFromDatabase(string itemListId)
        {
            var currencySlotPosition = -1;
            var catalog = new MerchantCatalog();
            var dbMerchantItems = DOLDB<MerchantItem>.SelectObjects(DB.Column(nameof(MerchantItem.ItemListID)).IsEqualTo(itemListId));
            foreach (var dbMerchantItem in dbMerchantItems)
            {
                var page = catalog.GetPage(dbMerchantItem.PageNumber);
                if (dbMerchantItem.SlotPosition == currencySlotPosition)
                {
                    var currencyId = (eCurrency)dbMerchantItem.Price;
                    if (currencyId == eCurrency.ItemTemplate)
                    {
                        var currencyItem = GameServer.Database.FindObjectByKey<ItemTemplate>(dbMerchantItem.ItemTemplateID);
                        if (currencyItem == null) throw new NullReferenceException($"The currency item {dbMerchantItem.ItemTemplateID} for item list {itemListId} on page {dbMerchantItem.PageNumber} could not be loaded");
                        page.SetCurrency(Money.Item(currencyItem));
                    }
                    else page.SetCurrency(Currency.Create((eCurrency)currencyId));
                }
                var itemTemplate = GameServer.Database.FindObjectByKey<ItemTemplate>(dbMerchantItem.ItemTemplateID);
                if (itemTemplate == null) continue;
                var currencyAmount = dbMerchantItem.Price != 0 ? dbMerchantItem.Price : itemTemplate.Price;
                catalog.GetPage(dbMerchantItem.PageNumber).Add(itemTemplate, (byte)dbMerchantItem.SlotPosition);
            }
            catalog.ItemListId = itemListId;
            return catalog;
        }

        public IEnumerable<MerchantCatalogEntry> GetAllEntries()
            => merchantPages.Select(x => x.Value.GetAllEntries()).SelectMany(x => x);

        public MerchantCatalogPage GetPage(int pageNumber)
        {
            var pageIsInvalid = pageNumber < FIRST_PAGE || pageNumber >= MAX_PAGES;
            if (pageIsInvalid) throw new ArgumentException($"PageNumber {pageNumber} is invalid. PageNumber must be in the range of 0-4.");
            if (merchantPages.TryGetValue((byte)pageNumber, out var page) == false)
            {
                merchantPages.Add((byte)pageNumber, new MerchantCatalogPage((byte)pageNumber));
            }
            return merchantPages[(byte)pageNumber];
        }

        public IEnumerable<MerchantCatalogPage> GetAllPages()
            => merchantPages.Values;

        public MerchantCatalogEntry GetEntry(int atPage, int atSlot)
            => GetPage(atPage).GetEntry((byte)atSlot);

        public bool IsEmpty
            => merchantPages.Count == 0;

        public MerchantTradeItems ConvertToMerchantTradeItems()
            => new MerchantTradeItems(this);
    }
}