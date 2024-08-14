using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MantisAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl("https://mantis-prova.base2.com.br/my_view_page.php");

                driver.Manage().Window.Maximize();

                Mapeamento.Login(driver, "username", "password");

                RunTest_CreatingTask(driver);
                RunTest_VerifyCreatedTask(driver);
                RunTest_AddNotation(driver);
            }
            finally
            {
                driver.Quit();
            }
        }
        // Caso de Teste 01 - Criar uma tarefa
        static void RunTest_CreatingTask(IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Iniciando o Caso de Teste: Criar uma tarefa");

                driver.Navigate().GoToUrl("https://mantis-prova.base2.com.br/bug_report_page.php");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(By.Id("category_id")).Displayed);

                IWebElement categoryDropdown = driver.FindElement(By.Id("category_id"));

                SelectElement selectCategory = new SelectElement(categoryDropdown);

                selectCategory.SelectByText("[Todos os Projetos] nova categoria");

                IWebElement summaryField = driver.FindElement(By.Id("summary"));
                summaryField.SendKeys("Teste Caso 1");

                IWebElement descriptionField = driver.FindElement(By.Id("description"));
                descriptionField.SendKeys("Esta é a descrição do Teste Caso 1");

                IWebElement submitTaskButton = driver.FindElement(By.XPath("//input[@value='Criar Nova Tarefa']"));
                submitTaskButton.Click();

                driver.Navigate().GoToUrl("https://mantis-prova.base2.com.br/my_view_page.php");

                wait.Until(d => d.FindElement(By.ClassName("widget-body")).Displayed);

                var functionReturn = Mapeamento.GetActivityDetails(driver, 2, "criou a tarefa");

                if (functionReturn.Username == "username" && functionReturn.extractedText == "criou a tarefa")
                {
                    Console.WriteLine("Tarefa inserida com sucesso");
                }
                else
                {
                    Console.WriteLine("Falha ao inserir uma Tarefa");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante o Caso de Teste - Criar uma tarefa: {ex.Message}");
            }
        }

        // Caso de Teste 02 - Verificar a tarefa criada
        static void RunTest_VerifyCreatedTask(IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Iniciando o Caso de Teste: Verificar a tarefa criada");

                driver.Navigate().GoToUrl("https://mantis-prova.base2.com.br/view_all_bug_page.php");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(By.Id("buglist")).Displayed);

                IWebElement taskTable = driver.FindElement(By.Id("buglist"));
                bool taskFound = false;

                foreach (var row in taskTable.FindElements(By.TagName("tr")))
                {
                    var cells = row.FindElements(By.CssSelector("td"));

                    if (cells.Count > 0)
                    {
                        var i = Mapeamento.Celula(driver, "Resumo");


                        var summaryCell = cells[i];

                        if (summaryCell.Text.Contains("Teste Caso 1"))
                        {
                            Console.WriteLine("Tarefa encontrada: " + summaryCell.Text);
                            taskFound = true;
                            break;
                        }
                    }
                }

                if (taskFound)
                {
                    Console.WriteLine("A tarefa criada foi encontrada na lista de tarefas.");
                }
                else
                {
                    Console.WriteLine("A tarefa criada NÃO foi encontrada na lista de tarefas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante o Caso de Teste - Verificar a tarefa criada: {ex.Message}");
            }
        }
        //Caso de Teste 03 - Adicionar uma nota em uma tarefa
        static void RunTest_AddNotation(IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Iniciando o Caso de Teste: Adiciona anotação");

                driver.Navigate().GoToUrl("https://mantis-prova.base2.com.br/view_all_bug_page.php");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(By.Id("buglist")).Displayed);

                IWebElement taskTable = driver.FindElement(By.Id("buglist"));
                Console.WriteLine(taskTable);

                IWebElement grid = Mapeamento.gridRow(driver, 1);
                var cells = grid.FindElements(By.CssSelector("td"));

                if (cells.Count > 0)
                {
                    int i = Mapeamento.Celula(driver, "Seleção");

                    var summaryCell = cells[i];

                    IWebElement elemento = driver.FindElement(By.CssSelector("span.lbl"));

                    elemento.Click();
                }

                IWebElement typeSelection = driver.FindElement(By.Name("action"));

                SelectElement selectAnotation = new SelectElement(typeSelection);

                selectAnotation.SelectByText("Adicionar Anotação");

                IWebElement submitButton = driver.FindElement(By.CssSelector("input.btn.btn-primary.btn-white.btn-sm.btn-round[value='OK']"));

                submitButton.Click();

                wait.Until(d => d.FindElement(By.Id("bugnote_text")).Displayed);

                IWebElement noteGrid = driver.FindElement(By.Id("bugnote_text"));

                noteGrid.SendKeys("Caso Teste 03");

                IWebElement buttonAddAnotation = driver.FindElement(By.CssSelector("input.btn.btn-primary.btn-white.btn-round[value='Adicionar Anotação']"));
                buttonAddAnotation.Click();

                driver.Navigate().GoToUrl("https://mantis-prova.base2.com.br/my_view_page.php");

                wait.Until(d => d.FindElement(By.ClassName("widget-body")).Displayed);

                var functionReturn = Mapeamento.GetActivityDetails(driver, 1, "comentou a tarefa");

                if (functionReturn.Username == "username" && functionReturn.extractedText == "comentou a tarefa")
                {
                    Console.WriteLine("Anotação inserida com sucesso");
                }
                else
                {
                    Console.WriteLine("Falha ao inserir uma anotação");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante o Caso de Teste - Adiciona anotação: {ex.Message}");
            }
        }
    }
}