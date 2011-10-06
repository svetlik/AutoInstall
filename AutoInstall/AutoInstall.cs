using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Windows.Automation;

namespace Svetlik
{
    class UIAutomation
    {
        private static string step;
        private static string windowTitle;

        static void Main(string[] args)
        {
            windowTitle = "Infragistics NetAdvantage Windows Forms 2011.1";
            step = "start";

            AutomationEventHandler eventHandler = new AutomationEventHandler(OnWindowOpen);
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Descendants, eventHandler);

            System.Diagnostics.Process.Start("C:/Install/NetAdvantage_WinForms_20111_JP.msi");//non-blocking

            Console.WriteLine("Press any key to stop automating...");
            Console.ReadLine();//blocking - dokato ne se sluchi neshto ne vrysha rezultat
        }

        private static void OnWindowOpen(object src, AutomationEventArgs e)
        {
            AutomationElement sourceElement;
            string windowName;
            try
            {
                sourceElement = src as AutomationElement;
                windowName = sourceElement.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
            }
            catch (ElementNotAvailableException)
            {
                return;
            }

            Console.WriteLine("Window opened: " + windowName);

            if (windowName == windowTitle)
            {
                Automate();
            }
        }

       
        private static bool ButtonClick(AutomationElement inElement, string automationId)
        {
            PropertyCondition btnCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, automationId);

            Console.WriteLine("Searching for the {0} button...", automationId);
            AutomationElement control = inElement.FindFirst(TreeScope.Descendants, btnCondition);
            if (control != null)
            {
                Console.WriteLine("OK.");
                Console.WriteLine("Clicking the {0} button", automationId);

                object controlType = control.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty);
                if (controlType == ControlType.Button)
                {
                    InvokePattern clickCommand = control.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                    clickCommand.Invoke();
                }
                else if (controlType == ControlType.RadioButton)
                {
                    SelectionItemPattern radioCheck = control.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
                    radioCheck.Select(); 
                }
                Console.WriteLine("OK.");

                return true;
            }
            else
            {
                Console.WriteLine("Error");
                return false;
            }
        }

        private static void Automate()
        {
            Console.WriteLine("Getting RootElement...");
            AutomationElement rootElement = AutomationElement.RootElement;

            if (rootElement != null)
            {
                Console.WriteLine("OK.");

                Condition condition = new PropertyCondition(AutomationElement.NameProperty, windowTitle);

                Console.WriteLine("Searching for {0} Window...", windowTitle);
                AutomationElement appElement = rootElement.FindFirst(TreeScope.Children, condition);

                if (appElement != null)
                {
                    Console.WriteLine("OK.");
                    string buttonToClick = "512";
                    switch (step)
                    {
                        case "start":
                            step = "licence";
                            break;

                        case "licence":
                            ButtonClick(appElement, "604");
                            System.Threading.Thread.Sleep(1000);
                            step = "optionalInstall";
                            break;

                        case "optionalInstall":
                            ButtonClick(appElement, "604");
                            System.Threading.Thread.Sleep(1000);
                            step = "userDetails";
                            break;

                    }

                    ButtonClick(appElement, buttonToClick);
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}