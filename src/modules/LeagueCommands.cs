using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;

namespace AllstarsBot.modules {
    [Group("LeagueBot")]
    public class LeagueCommands : ModuleBase<SocketCommandContext> {
        private static readonly Dictionary<string, string> ChampionIcons = new() {
            {"Anivia", "<:Anivia:866006734585462785>"},
            {"Azir", "<:Azir:866006734661222451>"},
            {"Alistar", "<:Alistar:866006734736326717>"},
            {"Aatrox", "<:Aatrox:866006734736457730>"},
            {"Annie", "<:Annie:866006734748778508>"},
            {"JarvanIV", "<:JarvanIV:866006734748909589>"},
            {"Fizz", "<:Fizz:866006734794784829>"},
            {"Garen", "<:Garen:866006734811037707>"},
            {"Draven", "<:Draven:866006734837645383>"},
            {"Akali", "<:Akali:866006734942502962>"},
            {"Brand", "<:Brand:866006734949580842>"},
            {"Amumu", "<:Amumu:866006734957707274>"},
            {"Corki", "<:Corki:866006734971994132>"},
            {"Caitlyn", "<:Caitlyn:866006734975533146>"},
            {"Ashe", "<:Ashe:866006734982873088>"},
            {"Blitzcrank", "<:Blitzcrank:866006734983135242>"},
            {"Evelynn", "<:Evelynn:866006735001223228>"},
            {"Fiddlesticks", "<:Fiddlesticks:866006735005024276>"},
            {"Galio", "<:Galio:866006735025864714>"},
            {"Bard", "<:Bard:866006735026257940>"},
            {"Fiora", "<:Fiora:866006735033597993>"},
            {"Chogath", "<:Chogath:866006735034777610>"},
            {"DrMundo", "<:DrMundo:866006735034777640>"},
            {"Irelia", "<:Irelia:866006735046705152>"},
            {"Heimerdinger", "<:Heimerdinger:866006735050899496>"},
            {"Janna", "<:Janna:866006735051685888>"},
            {"AurelionSol", "<:AurelionSol:866006735058763787>"},
            {"Camille", "<:Camille:866006735060074556>"},
            {"Diana", "<:Diana:866006735068463104>"},
            {"Jayce", "<:Jayce:866006735080783872>"},
            {"Gangplank", "<:Gangplank:866006735083536404>"},
            {"Ekko", "<:Ekko:866006735084978197>"},
            {"Hecarim", "<:Hecarim:866006735091925012>"},
            {"Gragas", "<:Gragas:866006735109226526>"},
            {"Aphelios", "<:Aphelios:866006735113945098>"},
            {"Elise", "<:Elise:866006735118139412>"},
            {"Gwen", "<:Gwen:866006735118794790>"},
            {"Ahri", "<:Ahri:866006735122464818>"},
            {"Darius", "<:Darius:866006735143043072>"},
            {"Ezreal", "<:Ezreal:866006735151169536>"},
            {"Jinx", "<:Jinx:866006735152087070>"},
            {"Kaisa", "<:Kaisa:866006735163883530>"},
            {"Ivern", "<:Ivern:866006735180529672>"},
            {"Jhin", "<:Jhin:866006735219064862>"},
            {"Jax", "<:Jax:866006735221948456>"},
            {"Graves", "<:Graves:866006735222341632>"},
            {"Gnar", "<:Gnar:866006735235711036>"},
            {"Cassiopeia", "<:Cassiopeia:866006735235842127>"},
            {"Braum", "<:Braum:866006735243575306>"},
            {"Illaoi", "<:Illaoi:866006735340830721>"},
            {"Lillia", "<:Lillia:866006892785434665>"},
            {"Leona", "<:Leona:866006892820430879>"},
            {"Nidalee", "<:Nidalee:866006892911001683>"},
            {"Kennen", "<:Kennen:866006892920045609>"},
            {"Maokai", "<:Maokai:866006892978896919>"},
            {"Lucian", "<:Lucian:866006892979683340>"},
            {"Leblanc", "<:Leblanc:866006893045219329>"},
            {"Renekton", "<:Renekton:866006893079429141>"},
            {"Pantheon", "<:Pantheon:866006893108133889>"},
            {"Kayle", "<:Kayle:866006893146538004>"},
            {"Kalista", "<:Kalista:866006893150732308>"},
            {"Khazix", "<:Khazix:866006893154271252>"},
            {"Kayn", "<:Kayn:866006893154533396>"},
            {"Karthus", "<:Karthus:866006893155450901>"},
            {"LeeSin", "<:LeeSin:866006893158596648>"},
            {"Karma", "<:Karma:866006893163053076>"},
            {"Mordekaiser", "<:Mordekaiser:866006893188743178>"},
            {"Kindred", "<:Kindred:866006893200801822>"},
            {"Morgana", "<:Morgana:866006893201195038>"},
            {"Malzahar", "<:Malzahar:866006893205127178>"},
            {"Orianna", "<:Orianna:866006893205389379>"},
            {"Nocturne", "<:Nocturne:866006893209714758>"},
            {"Katarina", "<:Katarina:866006893210107944>"},
            {"Nasus", "<:Nasus:866006893217448027>"},
            {"Quinn", "<:Quinn:866006893221642291>"},
            {"Olaf", "<:Olaf:866006893230162000>"},
            {"Rengar", "<:Rengar:866006893246677062>"},
            {"Rakan", "<:Rakan:866006893248249886>"},
            {"MissFortune", "<:MissFortune:866006893252050985>"},
            {"Rammus", "<:Rammus:866006893255065610>"},
            {"KogMaw", "<:KogMaw:866006893260439582>"},
            {"Rell", "<:Rell:866006893260832778>"},
            {"MonkeyKing", "<:MonkeyKing:866006893276954644>"},
            {"Nunu", "<:Nunu:866006893280886845>"},
            {"Kled", "<:Kled:866006893281411092>"},
            {"Lulu", "<:Lulu:866006893285212180>"},
            {"MasterYi", "<:MasterYi:866006893298319370>"},
            {"Nami", "<:Nami:866006893298319420>"},
            {"Ornn", "<:Ornn:866006893306576896>"},
            {"RekSai", "<:RekSai:866006893315227648>"},
            {"Lux", "<:Lux:866006893315358720>"},
            {"Lissandra", "<:Lissandra:866006893348651038>"},
            {"Poppy", "<:Poppy:866006893355991050>"},
            {"Riven", "<:Riven:866006893378011196>"},
            {"Nautilus", "<:Nautilus:866006893406584832>"},
            {"Pyke", "<:Pyke:866006893418512414>"},
            {"Qiyana", "<:Qiyana:866006893436076072>"},
            {"Neeko", "<:Neeko:866006893452197918>"},
            {"Kassadin", "<:Kassadin:866006893548535829>"},
            {"Malphite", "<:Malphite:866006893582876672>"},
            {"Taric", "<:Taric:866007361729724417>"},
            {"Trundle", "<:Trundle:866007361801420812>"},
            {"Teemo", "<:Teemo:866007361809416214>"},
            {"Sett", "<:Sett:866007361847558155>"},
            {"Vladimir", "<:Vladimir:866007361851490305>"},
            {"Xerath", "<:Xerath:866007361867874315>"},
            {"Syndra", "<:Syndra:866007361872461876>"},
            {"Velkoz", "<:Velkoz:866007361877311519>"},
            {"Veigar", "<:Veigar:866007361885569039>"},
            {"Yorick", "<:Yorick:866007361897234463>"},
            {"Talon", "<:Talon:866007361911390229>"},
            {"Yuumi", "<:Yuumi:866007361948352574>"},
            {"Viego", "<:Viego:866007361948745819>"},
            {"Warwick", "<:Warwick:866007362028699659>"},
            {"Rumble", "<:Rumble:866007362032762910>"},
            {"Sion", "<:Sion:866007362036301904>"},
            {"Ryze", "<:Ryze:866007362040627290>"},
            {"Shyvana", "<:Shyvana:866007362041151498>"},
            {"Seraphine", "<:Seraphine:866007362049671200>"},
            {"Sivir", "<:Sivir:866007362053603408>"},
            {"Skarner", "<:Skarner:866007362056749116>"},
            {"Sejuani", "<:Sejuani:866007362069594132>"},
            {"Singed", "<:Singed:866007362074837002>"},
            {"Shen", "<:Shen:866007362077589534>"},
            {"Samira", "<:Samira:866007362079031296>"},
            {"Tristana", "<:Tristana:866007362082439168>"},
            {"Varus", "<:Varus:866007362095546378>"},
            {"Tryndamere", "<:Tryndamere:866007362102755328>"},
            {"Twitch", "<:Twitch:866007362104066078>"},
            {"Taliyah", "<:Taliyah:866007362120974366>"},
            {"Sylas", "<:Sylas:866007362128314389>"},
            {"Zac", "<:Zac:866007362128707647>"},
            {"Yasuo", "<:Yasuo:866007362132377671>"},
            {"Volibear", "<:Volibear:866007362136309760>"},
            {"Swain", "<:Swain:866007362137620550>"},
            {"Yone", "<:Yone:866007362140897290>"},
            {"Thresh", "<:Thresh:866007362153349160>"},
            {"Xayah", "<:Xayah:866007362154659840>"},
            {"Senna", "<:Senna:866007362162786344>"},
            {"Soraka", "<:Soraka:866007362167242762>"},
            {"Urgot", "<:Urgot:866007362178777108>"},
            {"Vi", "<:Vi:866007362191491142>"},
            {"Shaco", "<:Shaco:866007362199224350>"},
            {"XinZhao", "<:XinZhao:866007362199224370>"},
            {"Vayne", "<:Vayne:866007362212593694>"},
            {"TahmKench", "<:TahmKench:866007362229108736>"},
            {"Viktor", "<:Viktor:866007362237890611>"},
            {"Udyr", "<:Udyr:866007362259124264>"},
            {"Sona", "<:Sona:866007362266595328>"},
            {"TwistedFate", "<:TwistedFate:866007362351005726>"},
            {"Ziggs", "<:Ziggs:866007540101021697>"},
            {"Zed", "<:Zed:866007540508262400>"},
            {"Zyra", "<:Zyra:866007540554792980>"},
            {"Zoe", "<:Zoe:866007540747468801>"},
            {"Zilean", "<:Zilean:866007541010661396>"},
        };

        [NamedArgumentType]
        public class GameArguments {
            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public IEnumerable<string> Bans { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public IEnumerable<string> FilterPlayers { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public IEnumerable<string> Players { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public IEnumerable<string> Team1 { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public IEnumerable<string> Team2 { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Global
            public int ChampionAmount { get; set; }
        }

        [Command("new")]
        [Alias("game")]
        [Summary("Rolls new Champions and Teams")]
        public async Task NewGame(GameArguments args = null) {
            args ??= new GameArguments();

            var rnd = new Random();
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"League Intern Champion Rollercoaster")
                .WithCurrentTimestamp();

            var players = new List<string>();
            var playersTeamOne = new List<string>();
            var playersTeamTwo = new List<string>();

            if (args.Team1 != null) {
                if (args.Team2 == null) {
                    await ReplyAsync("Error - Cannot Define Team1 without defining Team2");
                    return;
                }

                playersTeamOne = args.Team1.ToList();
                playersTeamTwo = args.Team2.ToList();
            } else if (args.Players == null) {
                var channel = (Context.User as IGuildUser)?.VoiceChannel;

                if (channel != null) {
                    var users = Context.Guild.GetVoiceChannel(channel.Id).Users;

                    players.AddRange(users
                        .Where(u => args.FilterPlayers == null || (args.FilterPlayers != null &&
                                                                   !(args.FilterPlayers.Contains(u.Nickname) ||
                                                                     args.FilterPlayers.Contains(u.Username))))
                        .Select(u => u.Nickname ?? u.Username));
                }
            } else if (players.Count == 0) {
                players = args.Players.ToList();
            }

            var allChamps = GetChampions();
            var champsSortedRandomly = new string[allChamps.Keys.Count];

            allChamps.Keys.CopyTo(champsSortedRandomly, 0);
            champsSortedRandomly = champsSortedRandomly.OrderBy(x => rnd.Next()).ToArray();

            var champsPerTeam = 12;
            if (args.ChampionAmount > 0) {
                champsPerTeam = args.ChampionAmount;
            } else if (players.Count > 0) {
                champsPerTeam = players.Count / 2 + 2;
            }

            var teamOne = new List<string>();
            var teamTwo = new List<string>();

            var i = 0;
            var rollTeamTwo = false;

            ImmutableHashSet<string> bans = null;
            if (args.Bans != null) {
                bans = args.Bans.ToImmutableHashSet();
            }

            if (bans != null && champsPerTeam > ((allChamps.Count - bans.Count) / 2)) {
                await ReplyAsync("Error - Champs Per team is higher than ([ChampionCount]-[BanCount])/2");
                return;
            } else if (champsPerTeam > ((allChamps.Count) / 2)) {
                await ReplyAsync("Error - Champs Per team is higher than ([ChampionCount]-[BanCount])/2");
                return;
            }

            for (var j = 0; j < (champsPerTeam * 2); j++) {
                if (j == champsPerTeam && !rollTeamTwo) {
                    rollTeamTwo = true;
                }

                i = GetChampionNumber(i, champsSortedRandomly, bans);

                if (!rollTeamTwo) teamOne.Add(champsSortedRandomly[i]);
                else teamTwo.Add(champsSortedRandomly[i]);

                i++;
            }

            if (bans != null) {
                var banBuilder = new StringBuilder();
                foreach (var ban in bans) {
                    banBuilder.AppendLine($"{ChampionIcons[allChamps[ban]]} {ban}");
                }

                embedBuilder.AddField($"Bans", banBuilder.ToString());
            }

            if (players is {Count: > 0} && (playersTeamOne.Count == 0 && playersTeamTwo.Count == 0)) {
                players = players.OrderBy(a => rnd.Next()).ToList();
                playersTeamOne = players.Take(players.Count / 2).ToList();
                playersTeamTwo = players.Skip(players.Count / 2).ToList();

                if (playersTeamOne.Count > 0 && playersTeamTwo.Count > 0) {
                    var t1 = new StringBuilder();
                    foreach (var x in playersTeamOne) {
                        t1.AppendLine(x);
                    }

                    embedBuilder.AddField($"Team 1 Blue ({playersTeamOne.Count})", t1.ToString(), true);

                    var t2 = new StringBuilder();
                    foreach (var x in playersTeamTwo) {
                        t2.AppendLine(x);
                    }

                    embedBuilder.AddField($"Team 2 Red ({playersTeamTwo.Count})", t2.ToString(), true);
                    embedBuilder.AddField("\u200B", "\u200B", true);
                }
            }

            var blueTeam = TeamCreator(allChamps, teamOne);
            var redTeam = TeamCreator(allChamps, teamTwo);

            var index = 0;
            var next = "blue";
            var run = true;

            while (run) {
                switch (next) {
                    case "empty":
                        embedBuilder.AddField("\u200B", "\u200B", true);
                        next = "blue";
                        index++;
                        break;
                    case "blue":
                        embedBuilder.AddField(FieldHeader("Blue", index + 1, redTeam.Length, teamOne.Count),
                            blueTeam[index],
                            true);
                        next = "red";
                        break;
                    case "red":
                        embedBuilder.AddField(FieldHeader("Red", index + 1, redTeam.Length, teamTwo.Count),
                            redTeam[index],
                            true);
                        next = "empty";
                        break;
                }

                if (index == redTeam.Length) {
                    run = false;
                }
            }

            var embedBuild = embedBuilder.Build();
            await ReplyAsync(embed: embedBuild);
        }

        private static string[] _players = {
        };

        private static string[] _champs = {

        };
        
        [Command("help")]
        public async Task Help() {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"Kureq's Shitty Help Command")
                .WithFooter("Contact Kureq#4029 if you still need help")
                .WithCurrentTimestamp()
                .WithDescription(
                    "The command to create new teams is: ```!LeagueBot New```\nKeep in mind that if you have an uneven amount of players e.g. 5, the bot will always add an extra player to red team (team 2)");

            embedBuilder.AddField("Champions Per Team",
                "Usage: ```!LeagueBot New ChampionAmount:\"15\"```\ndefines the amount of Champions that each team gets to choose from.\n Default 12 without any players defined or [PlayerAmount + 2] if players are found.");
            embedBuilder.AddField("Chaining LeagueCommands",
                "You can chain commands like so: ```!Leaguebot New Bans:\"Camille,Lulu,Teemo\" Players:\"TrundleGod, Feedius, SapphireCrystal, ZileanIsDumb, FuckShaco, RockIsgood, HarryPulledAWilly, WhoEvenPlaysADC\" ChampionAmount:\"15\"```");

            embedBuilder.AddField("No Parameters",
                $"Without parameters it generates two team with 12 random champions.\nIf you're joined to a voice channel, the bot will try to pull the players from those who are in the channel.",
                false);

            embedBuilder.AddField("Bans",
                $"Usage: ```!LeagueBot New Bans:\"Camille,Veigar,Karthus,Yuumi\"```\nPrevents the target champions from being available. Remember the list have to be in double quotations.");

            embedBuilder.AddField("Filter Voice Chat Players",
                "Usage: ```!LeagueBot New Filter:\"Kureq,Snigeren,Ugimagimato,LydBot\"```\nBy Default, if no _players_ are defined, the bot will try to add all that are joined to the voice chat.\nBut if you want to filter players, in the voice chat, that for some reason, don't wanna play intern league(Fucking Illaoi Mains)\nRemember to add the music bots if you create teams from voice");

            embedBuilder.AddField("Custom Player List",
                "Usage: ```!LeagueBot New Players:\"TrundleGod, Feedius, SapphireCrystal, ZileanIsDumb, FuckShaco, RockIsgood, HarryPulledAWilly, WhoEvenPlaysADC\"```\nCreate custom players / nicknames, if you use this option the bot will not pull players from the Voice Chat.\n");

            embedBuilder.AddField("Predefined Teams",
                "Usage: ```!LeagueBot New Team1: \"Kureq, Helms, Lerchemus, Brandt, SapphireProphet\" Team2:\"Snigeren, FlyvendeSten, TrylleFjams, DevMozz, GuildMaster\"```\nPredefines the two teams, the bot will no longer try to create random teams, or pull from voice chat.\nYou need to define team1(Blue Team) and team2(Red Team)");
            var embedBuild = embedBuilder.Build();
            await ReplyAsync(embed: embedBuild);
        }

        private static int GetChampionNumber(int i, string[] champs, ICollection<string> bannedChamps = null) {
            var champ = champs[i];
            if (bannedChamps == null || !bannedChamps.Contains(champ)) return i;
            i++;
            return GetChampionNumber(i, champs, bannedChamps);
        }

        private static string FieldHeader(string team, int currentPart, int maxPart, int maxChamps) {
            var sb = new StringBuilder();
            var t = 1;
            if (team == "red") {
                t = 2;
            }

            sb.Append(currentPart == 1
                ? $"Team {t} {team} Champions ({maxChamps})"
                : $"Team {t} {team} Champs part {currentPart} of {maxPart}"); //"\u200B");

            return sb.ToString();
        }

        private static string[] TeamCreator(Dictionary<string, string> champs, List<string> team, int maxSize = 20) {
            var max = Enumerable.Repeat(maxSize, team.Count / maxSize).ToList();
            if (team.Count % maxSize != 0) {
                max.Add(team.Count % maxSize);
            }

            var arrs = new List<string>();
            var t = team.ToArray();
            var x = 0;

            foreach (var t1 in max) {
                var builder = new StringBuilder();
                for (var j = 0; j < t1; j++) {
                    builder.AppendLine($"{ChampionIcons[champs[t[x]]]} {t[x]}");
                    x++;
                }

                arrs.Add(builder.ToString());
            }

            return arrs.ToArray();
        }

        private Dictionary<string, string> GetChampions() {
            var client = new WebClient();
            client.Headers.Add("user-agent",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            var versions = client.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
            var release = JArray.Parse(versions)[0];
            var releaseString = $"http://ddragon.leagueoflegends.com/cdn/{release}/data/en_US/champion.json";
            var champions = client.DownloadString(releaseString);

            var parent = JObject.Parse(champions);
            var champsObj = parent.Value<JObject>("data").Properties();
            var champs = champsObj.ToDictionary(k => k.Value["name"].ToString(), v => v.Value["id"].ToString());
            return champs;
        }
    }
}