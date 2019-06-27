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
using DOL.GS;
using DOL.Language;
using DOL.UnitTests.GameServer;
using NUnit.Framework;
using NSubstitute;
using DOL.Database;

namespace DOL.Server.Tests
{	
	/// <summary>
	/// Unit test for the Language Mgr
	/// </summary>
	[TestFixture]
	public class LanguageTest: ServerTests
	{
		public LanguageTest()
		{
		}
		
		[Test]
		public void TestGetString()
		{
			Console.WriteLine("TestGetString();");
			Console.WriteLine(LanguageMgr.GetTranslation ("test","fail default string"));
			Assert.IsTrue(true, "ok");
		}

		[Test]
		public void GetLanguageDataObject_EN_GameNPCSayToSays_System_TranslationIDisGameNPCSayToSays()
		{
			string language = "EN";
			string translationId = "GameNPC.SayTo.Says";
			var translationIdentifier = LanguageDataObject.eTranslationIdentifier.eSystem;

			LanguageDataObject languageDataObject = LanguageMgr.GetLanguageDataObject(language,translationId, translationIdentifier);
			var actual = languageDataObject.TranslationId;

			Assert.AreEqual("GameNPC.SayTo.Says", actual);
		}

		[Test]
		public void GetTranslation_EN_GameNPCSayToSays_ReturnSayString()
		{
			string language = "EN";
			string translationId = "GameNPC.SayTo.Says";
			var translationIdentifier = LanguageDataObject.eTranslationIdentifier.eSystem;

			string actual = LanguageMgr.GetTranslation(language, translationId);

			Assert.AreEqual("{0} says, \"{1}\"", actual);
		}

		[Test]
		public void GetTranslation_ClientExtension_AccountLanguageEN_GameNPCSayToSays_()
		{
			var client = new GameClient(null);
			client.Account = new Account();
			client.Account.Language = "EN";
			var npc = Create.FakeNPC();
			npc.TranslationId = "GameNPC.SayTo.Says";
			GS.ServerProperties.Properties.SERV_LANGUAGE = "FR";

			string actual = LanguageMgr.GetTranslation(client, npc).TranslationId;

			Assert.AreEqual("GameNPC.SayTo.Says", actual);
		}
	}
}
