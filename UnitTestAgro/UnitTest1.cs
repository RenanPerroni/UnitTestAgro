using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using NUnit.Framework;
using System;
using System.Threading;
using OpenQA.Selenium.DevTools.V125.Database;

namespace UnitTestAgro
{
    [TestFixture]
    public class AutomationWeb
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            //abre o browser edge
            driver = new EdgeDriver();
            Loginpage();
        }

        [TearDown]
        public void TeardownBrowser()
        {
            //ao finalizar o teste, fecha o browser
            driver.Quit();
        }

        public void Loginpage()
        {
            //faz o login na página inicial
            driver.Navigate().GoToUrl("http://processoseletivo.agrometrikaweb.com.br/GoAgro");
            driver.FindElement(By.Name("Email")).SendKeys("renanteste11@gmail.com");
            driver.FindElement(By.Name("Senha")).SendKeys("11");
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("btn")).Click();
            Thread.Sleep(2000);
        }

        public void GoProprietarios()
        {
            // Vai para a página Proprietário
            driver.Navigate().GoToUrl("http://processoseletivo.agrometrikaweb.com.br/GoAgro/Proprietario");
            Thread.Sleep(2000);
        }

        public void CepEdit()
        {
            // Edita o primeiro cep da lista para 12345678 e salva
            driver.FindElement(By.XPath("/html/body/div[2]/div/table/tbody/tr[2]/td[12]/a[1]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("Cep")).Clear();
            driver.FindElement(By.Name("Cep")).SendKeys("12345678");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/div[2]/div/form/div/div[14]/div/input")).Click();
            Thread.Sleep(2000);
        }

        public void VerificateCep()
        {
            // verifica se o cep foi salvo corretametne
            var cepElement = driver.FindElement(By.XPath("/html/body/div[2]/div/table/tbody/tr[2]/td[5]"));
            Thread.Sleep(2000);
            string cepAtual = cepElement.Text;
            Thread.Sleep(2000);

            if (cepAtual == "12345678")
            {
                Console.WriteLine("O CEP foi salvo corretamente!");
            }
            else
            {
                throw new Exception("O CEP não corresponde ao esperado.");
            }
        }

        public void GoToFazenda()
        {
            // Vai para a página Fazenda
            driver.Navigate().GoToUrl("http://processoseletivo.agrometrikaweb.com.br/GoAgro/Fazenda");
        }

        public void ClickExluirFazenda()
        {
            // Clica para excluir a primeira fazenda
            driver.FindElement(By.XPath("/html/body/div[2]/div/table/tbody/tr[2]/td[5]/a[3]")).Click();
        }

        public void VerificateFazenda()
        {
            // verifica se a primera Fazenda da lista Fazenda foi exluída
            var fazendaExcluida = driver.FindElement(By.XPath("/html/body/div[2]/div/table/tbody/tr[2]/td[2]"));
            Thread.Sleep(2000);
            string fazenda = fazendaExcluida.Text;
            Thread.Sleep(2000);

            if (fazenda != "QQQ123")
            {
                Console.WriteLine("Fazenda Excluída corretemante");
            }
            else
            {
                throw new Exception("Não foi possível exlcuir a fazenda!");
            }
        }

        [Test] // esse teste valida se o cep é salvo corretamente
        public void TestCepEdit()
        {
            GoProprietarios();
            CepEdit();
            VerificateCep();
        }

        [Test]
        //Esse teste é um exemplo de como aparece no test explorer caso um teste falhe. é esperado que este teste falhe, pois há um bug onde não é possível visualizar ou excluir fazendas.
        //Este bug já foi reportado no relatório com o bug id "9". 
        public void TestExcluiFazenda()
        {
            GoToFazenda();
            Thread.Sleep(2000);
            ClickExluirFazenda();
            Thread.Sleep(2000);
            VerificateFazenda();
            Thread.Sleep(2000);
        }
    }
}
