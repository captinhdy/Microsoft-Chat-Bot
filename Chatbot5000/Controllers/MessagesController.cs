using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using NeuralNetwork;
using NeuralNetwork.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Chatbot5000
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        List<CardAction> cardActions = new List<CardAction>();
        static ClassificationNetwork network;
        public MessagesController()
        {
            cardActions.Add(new CardAction() { Title = "Yes", Value = "1" });
            cardActions.Add(new CardAction() { Title = "No", Value = "2" });

        }
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity reply = null;
                //try
                //{
                //    if (activity.Text == "Send Hero Card")
                //    {
                //        reply = activity.CreateReply("Message contains hero card");
                //        reply.AttachmentLayout = AttachmentLayoutTypes.List;
                //        reply.Attachments = new List<Attachment>();
                //        HeroCard card = new HeroCard()
                //        {
                //            Title = "Do you like bots?",
                //            Subtitle = "Sparkhoud Blog",
                //            Buttons = cardActions
                //        };
                //        Attachment plAttachment = card.ToAttachment();
                //        reply.Attachments.Add(plAttachment);
                //    }
                //    else
                //    {
                //        string postData = "{\"documents\": [" +
                //                                            "{" +
                //                                                "\"language\": \"en\"," +
                //                                                "\"id\": \"1\"," +
                //                                                "\"text\": \"" + activity.Text + "\"" +
                //                                            "}"+
                //                            "]}";
                //        HTTPRequests httpRequests = new HTTPRequests("https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases");
                //        var keyWords = await httpRequests.PostRequest(postData);
                //        reply = activity.CreateReply(await keyWords.Content.ReadAsStringAsync());

                //    }
                //}
                //catch (Exception ex)
                //{
                //    reply = activity.CreateReply("There was an error processing your request: " + ex.Message);
                //}

                try
                {
                    reply = activity.CreateReply("Hello bot");
                    //await Conversation.SendAsync(activity, () => new RootDialog());

                    await connector.Conversations.SendToConversationAsync(reply);
                }
                catch (Exception ex)
                {
                    reply = activity.CreateReply("There was an error processing your request: " + ex.Message);
                }
                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                Activity reply = null;
                reply.CreateReply("Welcome to the chat");
                return reply;
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}