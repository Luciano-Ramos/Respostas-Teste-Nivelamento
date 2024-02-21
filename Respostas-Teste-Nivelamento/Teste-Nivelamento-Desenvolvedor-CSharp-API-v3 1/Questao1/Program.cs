using System;
using System.Collections.Generic;
using System.Globalization;

namespace Questao1
{
    class Program
    {
        private static int numero;
        private static ContaBancaria conta;
        private static List<ContaBancaria> contas = new List<ContaBancaria>();

        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("\n === Selecione uma opção === \n ");

                Console.WriteLine("1. Abertura de conta");
                Console.WriteLine("2. Depositar valor");
                Console.WriteLine("3. Sacar valor");
                Console.WriteLine("4. Alterar nome do titular da conta");
                Console.WriteLine("5. Detalhes da Conta");
                Console.WriteLine("6. Sair");
                Console.WriteLine("\n ==============================");

                int opcao;
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        // Abertura de conta 
                        AberturaConta(contas);
                        break;
                    case 2:
                        // Depositar valor
                        Deposito(contas);
                        break;
                    case 3:
                        // Sacar valor
                        Saque(contas);
                        break;
                    case 4:
                        //AlterarTitular
                        ValidarConta(contas);
                        if (conta == null)
                        {
                            Console.WriteLine("Conta Inexistente");
                        }
                        else
                        {
                            AlterarTitular();
                        }
                        break;
                    case 5:
                        // DetalheConta
                        ValidarConta(contas);
                        if (conta == null)
                        {
                            Console.WriteLine("\n Conta Inexistente");
                        }
                        else
                        {
                            Console.WriteLine("Dados da conta:");
                            Console.WriteLine(conta);
                        }
                        break;
                    case 6:
                        Console.WriteLine("Saindo do programa. Até mais!");
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }


            /* Output expected:
            Exemplo 1:

            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

            Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
            */

        }

        private static void AberturaConta(List<ContaBancaria> contas)
        {
            ValidarConta(contas);
            if (conta == null)
            {
                Console.Write("Entre o titular da conta: ");
                string titular = Console.ReadLine();
                Console.Write("Haverá depósito inicial (s/n)? ");
                //char resp = char.Parse(Console.ReadLine());

                char resp;
                while (true)
                {
                    string input = Console.ReadLine();

                    if (input.Length == 1 && char.TryParse(input, out resp))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Por favor, insira apenas um caractere (s/n).");
                    }
                }


                if (resp == 's' || resp == 'S')
                {
                    Console.Write("Entre com o valor de depósito inicial: ");

                    double depositoInicial;
                    while (!double.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out depositoInicial))
                    {
                        Console.WriteLine("Valor inválido. Por favor, insira um número válido.");
                        Console.Write("Entre com um valor para o depósito inicial: ");
                    }

                    ContaBancaria novaConta = new ContaBancaria(numero, titular, depositoInicial);
                    contas.Add(novaConta);
                }
                else
                {
                    ContaBancaria novaConta = new ContaBancaria(numero, titular);
                    contas.Add(novaConta);
                }
            }
            else
            {
                Console.WriteLine("Conta existente");
            }


            Console.WriteLine();
            Console.WriteLine("Dados da conta:");
            conta = ContaBancaria.BuscarConta(contas, numero);
            Console.WriteLine(conta);
        }


        private static void Saque(List<ContaBancaria> contas)
        {
            ValidarConta(contas);
            if (conta == null)
            {
                Console.WriteLine("Conta Inexistente");
            }
            else
            {
                Console.WriteLine();
                Console.Write("Entre um valor para saque: ");
                double quantia;
                while (!double.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out quantia))
                {
                    Console.WriteLine("Valor inválido. Por favor, insira um número válido.");
                    Console.Write("Entre com um valor para o saque: ");
                }
                conta.Saque(quantia);
                Console.WriteLine("Dados da conta atualizados:");
                Console.WriteLine(conta);
            }
        }

        private static void Deposito(List<ContaBancaria> contas)
        {
            ValidarConta(contas);
            if (conta == null)
            {
                Console.WriteLine("Conta Inexistente");
            }
            else
            {
                Console.WriteLine();
                Console.Write("Entre um valor para depósito: ");
                double quantia;
                while (!double.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out quantia))
                {
                    Console.WriteLine("Valor inválido. Por favor, insira um número válido.");
                    Console.Write("Entre com um valor para depósito: ");
                }
                conta.Deposito(quantia);
                Console.WriteLine("Dados da conta atualizados:");
                Console.WriteLine(conta);
            }

        }


        private static void AlterarTitular()
        {
            Console.WriteLine();
            Console.Write("Entre com novo nome do Titular: ");
            string novoNome = Console.ReadLine();
            conta.AlterarTitular(novoNome);
            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine(conta);
        }
        private static void ValidarConta(List<ContaBancaria> contas)
        {
            Console.Write("Entre com o número da conta: ");

            while (!int.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out numero))
            {
                Console.WriteLine("Valor inválido. Por favor, insira um número válido.");
                Console.Write("Entre o número da conta: ");
            }
            conta = ContaBancaria.BuscarConta(contas, numero);
        }

    }
}
