using CSharpHTMLUI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHTMLUI
{
    /// <summary>
    /// Handles the events send by the HTML-UI
    /// </summary>
    public class HTMLEventHandler
    {
        /// <summary>
        /// Contains all events
        /// </summary>
        private static List<EventTemplate> Events = new List<EventTemplate>();

        /// <summary>
        /// This method will call the event of the clicked item
        /// </summary>
        /// <param name="id">The id used to identify the clicked object</param>
        public static void HandleEvent(string id)
        {
            EventTemplate tmpEvent = GetEventById(id);
            if (tmpEvent != null)
            {
                tmpEvent.OnClick();
            }
        }

        /// <summary>
        /// Adds an event to the event list
        /// </summary>
        /// <param name="eventTemplate">The event to register</param>
        public static void RegisterEvent(EventTemplate eventTemplate)
        {
            Events.Add(eventTemplate);
        }

        /// <summary>
        /// Searches for an event with the given id
        /// </summary>
        /// <param name="id">The id to search for</param>
        /// <returns>The event with the matching id. Null if no event matched</returns>
        private static EventTemplate GetEventById(string id)
        {
            for (int i = 0; i < Events.Count; i++)
            {
                if (Events[i].GetId() == id)
                    return Events[i];
            }

            return null;
        }
    }
}
