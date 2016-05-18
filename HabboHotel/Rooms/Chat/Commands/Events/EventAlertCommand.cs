using Plus.Communication.Packets.Outgoing.Rooms.Notifications;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class EventAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_event_alert";
            }
        }
        public string Parameters
        {
            get
            {
                return "%message%";
            }
        }
        public string Description
        {
            get
            {
                return "Enviar um alerta de hotel para seu evento!";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session != null)
            {
                if (Room != null)
                {
                    if (Params.Length == 1)
                    {
                        Session.SendWhisper("Por favor, digite uma mensagem para enviar.");
                        return;
                    }
                    else
                    {
                        string Message = CommandManager.MergeParams(Params, 1);

                        PlusEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Está acontecendo um evento!",
                             "O Staff <b>" + Session.GetHabbo().Username +
                             "</b> está promovendo um evento no quarto dele. <br>O nome do evento é: <b>" + Message +
                             "</b> <br>Para participar, clique no botão abaixo:",
                             "events", "Participe agora mesmo!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                    }
                }
            }
        }
        /*public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session != null)
            {
                if (Room != null)
                {
                    if (Params.Length != 1)
                    {
                        Session.SendWhisper("Invalid command! :eventalert", 0);
                    }
                    else if (!PlusEnvironment.Event)
                    {
                        PlusEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(":follow " + Session.GetHabbo().Username + " for events! win prizes!\r\n- " + Session.GetHabbo().Username, ""), "");
                        PlusEnvironment.lastEvent = DateTime.Now;
                        PlusEnvironment.Event = true;
                    }
                    else
                    {
                        TimeSpan timeSpan = DateTime.Now - PlusEnvironment.lastEvent;
                        if (timeSpan.Hours >= 1)
                        {
                            PlusEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(":follow " + Session.GetHabbo().Username + " for events! win prizes!\r\n- " + Session.GetHabbo().Username, ""), "");
                            PlusEnvironment.lastEvent = DateTime.Now;
                        }
                        else
                        {
                            int num = checked(60 - timeSpan.Minutes);
                            Session.SendWhisper("Event Cooldown! " + num + " minutes left until another event can be hosted.", 0);
                        }
                    }
                }
            }
        }*/
    }
}