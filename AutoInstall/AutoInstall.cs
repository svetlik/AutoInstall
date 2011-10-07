using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Windows.Automation;

namespace Svetlik
{
    class UIAutomation
    {
        private static int step;
        private static string windowTitle;
        private static string[][] steps;
        
        static void Main()
        {   
            windowTitle = "Infragistics NetAdvantage Windows Forms 2011.1";

            string nextButton = "512"; 
            steps = new string[][]
            {
                new string[] {nextButton}, // start
                new string[] {"604", nextButton}, // license
                new string[] {"602", nextButton}, // optionalInstall
                new string[] {nextButton}, // userDetails
                new string[] {nextButton}, // installFolder
                new string[] {"578"}, // installType
                new string[] {"596"}, // installFinally
                new string[] {"734"}, // finishFinally
            };

            step = 0;

            AutomationEventHandler eventHandler = new AutomationEventHandler(OnWindowOpen);
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Descendants, eventHandler);

            System.Diagnostics.Process.Start("C:/Install/NetAdvantage_WinForms_20111_JP.msi");

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
                Console.WriteLine("Button {0} clicked.", automationId);

                return true;
            }
            else
            {
                Console.WriteLine("Could not find button {0} ", automationId);
                return false;
            }
        }

        private static void Automate()
        {
            Console.WriteLine("Getting RootElement...");
            AutomationElement rootElement = AutomationElement.RootElement;

            if (rootElement != null)
            {
                Condition condition = new PropertyCondition(AutomationElement.NameProperty, windowTitle);

                Console.WriteLine("Searching for {0} Window...", windowTitle);
                AutomationElement appElement = rootElement.FindFirst(TreeScope.Children, condition);

                if (appElement != null)
                {
                    foreach (string buttonId in steps[step])
                    {
                        if (!ButtonClick(appElement, buttonId))
                        {
                            Console.WriteLine("Could not find button.");
                            return;
                        }
                    }
                    step++;
                    Console.WriteLine("Moving to step {0}.", step);
                }
                else
                {
                    Console.WriteLine("Could not find the Installer Window.");
                }
            }
            else
            {
                Console.WriteLine("Could not get the RootElement.");
            }
        }
    }
}