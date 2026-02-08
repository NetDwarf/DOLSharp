using System;
using System.Collections.Generic;
using System.Linq;
using DOL.GS;
using DOL.Events;
using DOL.Database;
using log4net;

namespace DOL.GS.Scripts
{
    /// <summary>
    /// Manages nighttime spawning for Night Hobs
    /// Only spawns night hobs mobs during night hours
    /// Uses in-game time, not real-world time
    /// </summary>
    public class NightHobSpawnManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private static System.Timers.Timer m_checkTimer;
        private static List<GameNPC> m_nightHobs = new List<GameNPC>();
        private static readonly object m_lockObject = new object();
        
        // Configuration
        private const int CHECK_INTERVAL = 60000; // Check every minute (in milliseconds)
        private const ushort REGION_ID = 239;     // Target region
        private const int NIGHT_START_HOUR = 20;  // Night starts at 8:00 PM
        private const int NIGHT_END_HOUR = 6;     // Night ends at 6:00 AM
        
        /// <summary>
        /// Get current game hour (0-23)
        /// </summary>
        private static int GetGameHour()
        {
            uint dayTime = WorldMgr.GetCurrentGameTime();
            
            // GetCurrentGameTime returns milliseconds since game start
            // We need to extract the hour of the current day
            
            // Calculate milliseconds in a full day (24 hours)
            const uint MS_PER_DAY = 24 * 60 * 60 * 1000; // 86,400,000 ms
            
            // Get time within current day (modulo)
            uint timeOfDay = dayTime % MS_PER_DAY;
            
            // Convert to hours (0-23)
            int hour = (int)(timeOfDay / (60 * 60 * 1000)); // divide by ms per hour
            
            // log.Info($"NightHobSpawnManager: timeOfDay={timeOfDay}, hour={hour}");
            
            return hour;
        }
        
        /// <summary>
        /// Calculate if it's currently night time based on game time
        /// </summary>
        private static bool IsCurrentlyNight
        {
            get 
            { 
                int gameHour = GetGameHour();
                return IsNightTime(gameHour);
            }
        }
        
        [ScriptLoadedEvent]
        public static void OnScriptLoaded(DOLEvent e, object sender, EventArgs args)
        {
            log.Info("NightHobSpawnManager: Script loaded for region " + REGION_ID);
            Init();
        }
        
        [ScriptUnloadedEvent]
        public static void OnScriptUnloaded(DOLEvent e, object sender, EventArgs args)
        {
            log.Info("NightHobSpawnManager: Unloading script");
            if (m_checkTimer != null)
            {
                m_checkTimer.Stop();
                m_checkTimer.Dispose();
                m_checkTimer = null;
            }
            // Restore mobs to world when unloading script
            SpawnMobs();
        }
        
        public static void Init()
        {
            // Load all Night Hobs from the region
            LoadNightHobs();
            
            // Get current game time
            int gameHour = GetGameHour();
            log.Info($"NightHobSpawnManager: Current game time is {gameHour}:00");
            
            // Do initial spawn/despawn based on current game time
            if (IsCurrentlyNight)
            {
                log.Info("NightHobSpawnManager: Starting at night (game time) - mobs already present");
            }
            else
            {
                log.Info("NightHobSpawnManager: Starting during day (game time) - despawning mobs");
                DespawnMobs();
            }
            
            // Start the timer to check time periodically
			if (m_checkTimer == null)
			{
				m_checkTimer = new System.Timers.Timer(CHECK_INTERVAL);
				m_checkTimer.Elapsed += OnTimerElapsed;
				m_checkTimer.AutoReset = true;
				m_checkTimer.Start();
			}
            
            log.Info($"NightHobSpawnManager: Timer initialized, {m_nightHobs.Count} Night Hobs found");
        }
        
        /// <summary>
        /// Timer elapsed event handler
        /// </summary>
        private static void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckTimeAndManageSpawn();
        }
        
        /// <summary>
        /// Load all Night Hob mobs from the database in the specified region
        /// </summary>
        private static void LoadNightHobs()
        {
            m_nightHobs.Clear();
            
            // Get the region
            Region region = WorldMgr.GetRegion(REGION_ID);
            if (region == null)
            {
                log.Error("NightHobSpawnManager: Region " + REGION_ID + " not found");
                return;
            }
            
            // List of target mob names to search for
            var targetNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "night hob",
                "young night hob",
                "night hob shaman"
            };
            
            // Filter all NPCs in a single pass
            m_nightHobs = region.Objects
                .OfType<GameNPC>()
                .Where(npc => npc != null && targetNames.Contains(npc.Name))
                .ToList();
            
            if (m_nightHobs.Count == 0)
            {
                log.Warn($"NightHobSpawnManager: No Night Hobs found in region {REGION_ID}");
            }
            else
            {
                log.Info($"NightHobSpawnManager: {m_nightHobs.Count} Night Hobs loaded from region {REGION_ID}");
            }
        }
        
        /// <summary>
        /// Checks current game time and manages mob spawning
        /// </summary>
        private static void CheckTimeAndManageSpawn()
        {
            // If list is empty, reload
            if (m_nightHobs.Count == 0)
            {
                log.Warn("NightHobSpawnManager: Night Hobs list is empty, reloading...");
                LoadNightHobs();
                
                if (m_nightHobs.Count == 0)
                {
                    log.Error("NightHobSpawnManager: Still no Night Hobs found after reload!");
                    return;
                }
            }
            
            int gameHour = GetGameHour();
            bool shouldBeNight = IsNightTime(gameHour);
            bool mobsAreSpawned = AreMobsSpawned();
            
            if (shouldBeNight && !mobsAreSpawned)
            {
                // It's night and mobs are not spawned - spawn them
                log.Info($"NightHobSpawnManager: Night falls (game time {gameHour}:00), spawning {m_nightHobs.Count} Night Hobs");
                SpawnMobs();
            }
            else if (!shouldBeNight && mobsAreSpawned)
            {
                // It's day and mobs are spawned - despawn them
                log.Info($"NightHobSpawnManager: Dawn breaks (game time {gameHour}:00), despawning {m_nightHobs.Count} Night Hobs");
                DespawnMobs();
            }
        }
        
        /// <summary>
        /// Check if mobs are currently spawned in the world
        /// </summary>
        private static bool AreMobsSpawned()
        {
            lock (m_lockObject)
            {
                foreach (GameNPC mob in m_nightHobs)
                {
                    if (mob != null && mob.ObjectState == GameObject.eObjectState.Active)
                    {
                        return true; // At least one mob is active
                    }
                }
                return false; // No mobs are active
            }
        }
        
        /// <summary>
        /// Determines if given hour is considered night time
        /// </summary>
        private static bool IsNightTime(int hour)
        {
            // Night runs from NIGHT_START_HOUR to NIGHT_END_HOUR
            if (NIGHT_START_HOUR > NIGHT_END_HOUR)
            {
                // Night crosses midnight (e.g., 20h to 6h)
                return hour >= NIGHT_START_HOUR || hour < NIGHT_END_HOUR;
            }
            else
            {
                // Normal case
                return hour >= NIGHT_START_HOUR && hour < NIGHT_END_HOUR;
            }
        }
        
        /// <summary>
        /// Add all Night Hob mobs to the world
        /// </summary>
        private static void SpawnMobs()
        {
            lock (m_lockObject)
            {
                // Clean up null/invalid references
                m_nightHobs.RemoveAll(mob => mob == null || mob.ObjectState == GameObject.eObjectState.Deleted);
                
                int spawnedCount = 0;
                
                foreach (GameNPC mob in m_nightHobs)
                {
                    if (mob.ObjectState != GameObject.eObjectState.Active)
                    {
                        mob.AddToWorld();
                        spawnedCount++;
                    }
                }
                
                if (spawnedCount > 0)
                {
                    log.Info($"NightHobSpawnManager: {spawnedCount} Night Hobs spawned");
                }
            }
        }
        
        /// <summary>
        /// Remove all Night Hob mobs from the world
        /// </summary>
        private static void DespawnMobs()
        {
            lock (m_lockObject)
            {
                // Clean up null/invalid references
                m_nightHobs.RemoveAll(mob => mob == null || mob.ObjectState == GameObject.eObjectState.Deleted);
                
                int despawnedCount = 0;
                
                foreach (GameNPC mob in m_nightHobs)
                {
                    if (mob.ObjectState == GameObject.eObjectState.Active)
                    {
                        mob.RemoveFromWorld();
                        despawnedCount++;
                    }
                }
                
                if (despawnedCount > 0)
                {
                    log.Info($"NightHobSpawnManager: {despawnedCount} Night Hobs despawned");
                }
            }
        }
    }
}