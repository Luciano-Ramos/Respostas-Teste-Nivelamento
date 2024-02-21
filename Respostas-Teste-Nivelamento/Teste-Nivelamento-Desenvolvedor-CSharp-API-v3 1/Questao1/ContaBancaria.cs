using System.Collections.Generic;

namespace Questao1
{
    class ContaBancaria
    {
        private int numero;
        private string titular;


        private double saldo;
        private readonly double _taxa = 3.50;

        public ContaBancaria(int numero, string titular)
        {
            this.numero = numero;
            this.titular = titular;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            this.numero = numero;
            this.titular = titular;
            this.saldo = depositoInicial;
        }

        internal double Deposito(double quantia)
        {
            this.saldo += quantia;
            return this.saldo;
        }

        internal double Saque(double quantia)
        {
            this.saldo = (saldo - quantia) - _taxa;
            return this.saldo;
        }

        public static ContaBancaria BuscarConta(List<ContaBancaria> contas, int numeroConta)
        {
            foreach (var conta in contas)
            {
                if (conta.numero == numeroConta)
                {
                    return conta;
                }
            }
            return null;
        }

        public void AlterarTitular(string novoNome)
        {
            titular = novoNome;
        }

        public override string ToString()
        {
            return $"Conta {numero}, Titular: {titular}, Saldo: $ {saldo:F2}";
        }
    }
}
