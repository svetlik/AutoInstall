using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; //bez
using System.Linq; //?
using System.Text; //?

using System.Windows.Automation;

namespace Svetlik
{
    class UIAutomation
    {
        static void Main(string[] args)
        {
            AutomationEventHandler eventHandler = new AutomationEventHandler(OnWindowOpen);
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Descendants, eventHandler);

            //System.Diagnostics.Process.Start(args[0]);//non-blocking

            Console.WriteLine("Press any key to stop automating...");
            Console.ReadLine();//blocking - dokato ne se sluchi neshto ne vrysha rezultat
        }

        private static void OnWindowOpen(object src, AutomationEventArgs e)
        {
            AutomationElement sourceElement;
            try
            {
                sourceElement = src as AutomationElement;
            }
            catch (ElementNotAvailableException)
            {
                return;
            }

            string windowName = sourceElement.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
            Console.WriteLine("Window opened: " + windowName);

            if (windowName == "Infragistics NetAdvantage Windows Forms 2011.1")
            {
                Automate();
            }
        }

        private static void Automate()
        {
            Console.WriteLine("Getting RootElement...");
            AutomationElement rootElement = AutomationElement.RootElement;

            if (rootElement != null)
            {
                Console.WriteLine("OK.");

                Condition condition = new PropertyCondition(AutomationElement.NameProperty, "Infragistics NetAdvantage Windows Forms 2011.1");

                Console.WriteLine("Searching for Infragistics NetAdvantage Windows Forms 2011.1 Window...");
                AutomationElement appElement = rootElement.FindFirst(TreeScope.Children, condition);

                if (appElement != null)
                {
                    Console.WriteLine("OK.");

                    Condition btnTsugiCondition = new PropertyCondition(AutomationElement.NameProperty, "次へ(N) >");

                    Console.WriteLine("Searching for the TsugiHe(N) > button...");
                    AutomationElement btnTsugi = appElement.FindFirst(TreeScope.Descendants, btnTsugiCondition);
                    if (btnTsugi != null)
                    {
                        Console.WriteLine("OK.");

                        InvokePattern btnTsugiPattern = btnTsugi.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                        Console.WriteLine("Clicking the TsugiHe(N) > button");
                        btnTsugiPattern.Invoke();

                        Console.WriteLine("OK.");
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
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}