using System.Linq;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MassGiveCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mass_give"; }
        }

        public string Parameters
        {
            get { return "%type% %amount%"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public void Execute(GameClient session, Room room, string[] Params)
        {
            if (Params.Length == 1)
            {
                session.SendWhisper("Please enter a currency type! (coins, duckets, diamonds, gotw)");
                return;
            }

            var updateVal = Params[1];
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
                            foreach (var client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != session.GetHabbo().Username))
                            {
                                client.GetHabbo().Credits = client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));

                                if (client.GetHabbo().Id != session.GetHabbo().Id)
                                    client.SendNotification(session.GetHabbo().Username + " has given you " + amount +
                                                            " Credit(s)!");
                                session.SendWhisper("Successfully given " + amount + " Credit(s) to " +
                                                    client.GetHabbo().Username + "!");
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
                            foreach (var client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != session.GetHabbo().Username))
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));

                                if (client.GetHabbo().Id != session.GetHabbo().Id)
                                    client.SendNotification(session.GetHabbo().Username + " has given you " + amount +
                                                            " Ducket(s)!");
                                session.SendWhisper("Successfully given " + amount + " Ducket(s) to " +
                                                    client.GetHabbo().Username + "!");
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
                            foreach (var client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != session.GetHabbo().Username))
                            {
                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));

                                if (client.GetHabbo().Id != session.GetHabbo().Id)
                                    client.SendNotification(session.GetHabbo().Username + " has given you " + amount +
                                                            " Diamond(s)!");
                                session.SendWhisper("Successfully given " + amount + " Diamond(s) to " +
                                                    client.GetHabbo().Username + "!");
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
                            foreach (var client in PlusEnvironment.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != session.GetHabbo().Username))
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));

                                if (client.GetHabbo().Id != session.GetHabbo().Id)
                                    client.SendNotification(session.GetHabbo().Username + " has given you " + amount +
                                                            " GOTW Point(s)!");
                                session.SendWhisper("Successfully given " + amount + " GOTW point(s) to " +
                                                    client.GetHabbo().Username + "!");
                            }
                            break;
                        }
                        session.SendWhisper("Oops, that appears to be an invalid amount!");
                        break;
                    }
                default:
                    session.SendWhisper("'" + updateVal + "' is not a valid currency!");
                    break;
            }
        }
    }
}