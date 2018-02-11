using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;

namespace Chatbot5000
{
    [Serializable]
    public class SOWFormModel
    {
        [Prompt("What is the project name? {||}")]
        public string Name { get; set; }

        public static IForm<SOWFormModel> BuildForm()
        {
            return new FormBuilder<SOWFormModel>().Build();
        }

        public static IFormDialog<SOWFormModel> BuildFormDialog(FormOptions options = FormOptions.PromptInStart)
        {
            return FormDialog.FromForm(BuildForm, options);
        }
    }
}