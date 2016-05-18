using System.Linq;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;
using Plus.Database.Interfaces;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class GlobalGiveCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_global_currency"; }
        }

        public string Parameters
        {
            get { return "%type% %amount%"; }
        }

        public string Description
        {
            get { return "Update currencies for everyone in your database."; }
        }

        public void Execute(GameClient session, Room room, string[] Params)
        {
            if (Params.Length == 1)
            {
                session.SendWhisper("Please enter a currency type! (coins, duckets, diamonds, gotw)");
                return;
            }

            string updateVal = Params[1];
            switch (updateVal.ToLower())
            {
                case "coins":
                case "credits":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            session.SendWhisper("Oops, it appears that you do not have the permissions to use this command!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));
                            }
                            using (IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET credits = credits + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Oops, that appears to be an invalid amount!");
                        break;
                    }
                case "pixels":
                case "duckets":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            session.SendWhisper("Oops, it appears that you do not have the permissions to use this command!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));
                            }
                            using (IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET activity_points = activity_points + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Oops, that appears to be an invalid amount!");
                        break;
                    }
                case "diamonds":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            session.SendWhisper("Oops, it appears that you do not have the permissions to use this command!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));
                            }
                            using (IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET vip_points = vip_points + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Oops, that appears to be an invalid amount!");
                        break;
                    }
                case "gotw":
                case "gotwpoints":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            session.SendWhisper("Oops, it appears that you do not have the permissions to use this command!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));
                            }
                            using (IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET gotw_points = gotw_points + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Oops, that appears to be an invalid amount!");
                        break;
                    }
            }
        }
    }
}