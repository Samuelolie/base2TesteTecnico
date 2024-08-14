using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V125.Storage;
using OpenQA.Selenium.Support.UI;
using System;

namespace MantisAutomation
{
    public static class Mapeamento
    {
        public static void Login(IWebDriver driver, string username, string password)
        {
            IWebElement usernameField = driver.FindElement(By.Id("username"));
            usernameField.SendKeys(username);

            IWebElement loginButton = driver.FindElement(By.XPath("//input[@value='Entrar']"));
            loginButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement passwordField = wait.Until(d => d.FindElement(By.Id("password")));
            passwordField.SendKeys(password);

            IWebElement submitButton = driver.FindElement(By.XPath("//input[@value='Entrar']"));
            submitButton.Click();
        }

        public static int Celula(IWebDriver driver, string colunaTabela)
        {
            switch (colunaTabela)
            {
                case "Gravidade":
                    return 7;

                case "Resumo":
                    return 10;

                case "Seleção":
                    return 0;

                default:
                    return -1;
            }
        }

        public static IWebElement gridRow(IWebDriver driver, int linha)
        {
            IWebElement TabelaTarefa = driver.FindElement(By.Id("buglist"));

            return TabelaTarefa.FindElement(By.CssSelector($"tbody tr:nth-child({linha})"));
        }

        public static (string Username, string extractedText) GetActivityDetails(IWebDriver driver, int rowIndex, string searchText)
        {
            var activityElements = driver.FindElements(By.CssSelector("div.profile-activity.clearfix"));

            var activityElement = activityElements[rowIndex - 1];

            var username = activityElement.FindElement(By.CssSelector("span.username a")).Text;

            IWebElement actionElement = activityElement.FindElement(By.CssSelector("div.action"));

            string fullText = actionElement.Text;

            int startIndex = fullText.IndexOf(searchText);

            string extractedText = startIndex >= 0 ? fullText.Substring(startIndex, searchText.Length) : string.Empty;

            Console.WriteLine(extractedText);

            return (username, extractedText);
        }
    }

}
