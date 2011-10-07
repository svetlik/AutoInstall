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

        static void Main()
        {
            windowTitle = "Infragistics NetAdvantage Windows Forms 2011.1";
            step = "start";

            System.Diagnostics.Process.Start("C:/Install/NetAdvantage_WinForms_20111_JP.msi");

            AutomationEventHandler eventHandler = new AutomationEventHandler(OnWindowOpen);
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Descendants, eventHandler);

            

            //Console.WriteLine("Press any key to stop automating...");
            Console.ReadLine();
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
                System.Threading.Thread.Sleep(2000);
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
                            step = "begin";
                            break;

                        case "begin":
                            step = "license";
                            break;

                        case "license":
                            ButtonClick(appElement, "604");
                            System.Threading.Thread.Sleep(2000);
                            step = "optionalInstall";
                            break;

                        case "optionalInstall":
                            ButtonClick(appElement, "602");
                            System.Threading.Thread.Sleep(2000);
                            step = "userDetails";
                            break;

                        case "userDetails":
                            ButtonClick(appElement, "512");
                            System.Threading.Thread.Sleep(2000);
                            step = "installFolder";
                            break;

                        case "installFolder":
                            ButtonClick(appElement, "512");
                            System.Threading.Thread.Sleep(2000);
                            step = "installType";
                            break;

                        case "installType":
                            ButtonClick(appElement, "578");
                            System.Threading.Thread.Sleep(2000);
                            step = "installFinally";
                            break;

                        case "installFinally":
                            ButtonClick(appElement, "596");
                            System.Threading.Thread.Sleep(2000);
                            step = "finishInstall";
                            break;

                        case "finishInstall":
                            ButtonClick(appElement, "734");
                            System.Threading.Thread.Sleep(2000);
                            step = "final";
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