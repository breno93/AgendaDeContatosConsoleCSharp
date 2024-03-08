using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaConsole
{
    internal class Program
    {
        //Menu -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        static int ExibirMenu()
        {
            int op = 0;
            Console.Clear();
            Console.WriteLine("Agenda modo console");
            Console.WriteLine("Exibir contatos (1)");
            Console.WriteLine("Criar contato (2)");
            Console.WriteLine("Atualizar contato (3)");
            Console.WriteLine("Excluir contato (4)");
            Console.WriteLine("Localizar contato (5)");
            Console.WriteLine("Sair (6)");
            Console.Write("Opção: ");
            op = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            return op;
        }

        static void ExibirContatos(String[] nome, string[] email, int tl)
        {
            Console.WriteLine("Exibindo contatos");
            for (int i = 0; i < tl; i++)
            {
                Console.WriteLine("Nome: {0} - E-mail: {1}", nome[i], email[i]);
            }
            Console.ReadKey();
        }

        static void CriarContato(ref String[] nome, ref string[] email, ref int tl)
        {
            try
            {
                if (tl >= 200)
                {
                    Console.WriteLine("Tamanho maximo atingido");
                }
                else
                {
                    Console.WriteLine("Inserir contato");
                    Console.Write("Nome: ");
                    nome[tl] = Console.ReadLine();
                    Console.Write("Email: ");
                    email[tl] = Console.ReadLine();
                    int pos = LocalizarContato(email, tl, email[tl]);
                    if (pos == -1)
                    {
                        tl++;
                        Console.WriteLine("Contato cadastrado");
                    }
                    else
                    {
                        Console.WriteLine("Contato já cadastrado.");
                    
                    }
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro:" + e.Message);
                Console.ReadKey();
            }
        }

        static void AtualizarContato(ref string[] nome, ref string[] email, ref int tl)
        {
            try
            {
                Console.WriteLine("Atualizar contato");
                Console.Write("Email: ");
                string emailContato = Console.ReadLine();
                int pos = LocalizarContato(email, tl, emailContato);
                if (pos != -1)
                {
                    Console.WriteLine("Novos dados do contato");
                    Console.Write("Nome: ");
                    string novoNome = Console.ReadLine();

                    Console.Write("Email: ");
                    string novoEmail = Console.ReadLine();
                    int posValidacao = LocalizarContato(email, tl, novoEmail);
                    if (posValidacao == -1 || posValidacao == pos)
                    {
                        nome[pos] = novoNome;
                        email[pos] = novoEmail;
                        Console.WriteLine("Contato atualizado com sucesso");
                    }
                    else
                    {
                        Console.WriteLine("E-mail já cadastrado.");
                        
                    }
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Contato não encontrado");
                    Console.ReadKey();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro:" + e.Message);
                Console.ReadKey();
            }
            
        }

        static Boolean ExcluirContato(ref string[] nome, ref string[] email, ref int tl, string emailContato)
        {
            Boolean excluiu = false;
            int pos = -1;
            pos = LocalizarContato(email, tl, emailContato);
            if (pos != -1)
            {
                for (int i = pos; i < tl-1; i++)
                {
                    nome[i] = nome[i + 1];
                    email[i] = email[i + 1];
                }
                excluiu = true;
                tl--;
            }
            return excluiu; 
        }

        static int LocalizarContato(string[] email, int tl, string emailContato)
        {
            //pos é a posição que eu encontrei o email no meu array
            //e se nao encontrar nada, sera -1
            int pos = -1;
            int i = 0;
            while (i < tl && email[i] != emailContato)
            {
                i++;
            }
            if (i < tl) pos = i;
            return pos;
        }



        static void Main(string[] args)
        {
            //Armazena os dados da agenda
            String[] nome = new string[200];
            String[] email = new string[200];
            

            //Tamanho lógico
            int tl = 0;
            int op = 0;
            int pos = 0;
            String emailContato = "";

            //carregar dados do arquivo texto
            BackupAgenda.nomeArquivo = "dados.txt";
            BackupAgenda.RestaurarDados(ref nome, ref email, ref tl);

            
            while (op != 6)
            {
                op = ExibirMenu();
                
                switch (op)
                {
                    //Exibir os contatos -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                    case 1:
                        ExibirContatos(nome,email,tl);
                        break;

                    //Criar contato -=--=-=-=-=--=-=--=-=-=-=-=--=-=-=-=-=-=-=-=-=-=-=-
                    case 2:
                        CriarContato(ref nome, ref email, ref tl);
                        break;

                    //Atualizar contato -=-=-=-=-=-=-=-=-=-=-=---=-=-=-=-=-=-=-=-=-=-=
                    case 3:
                        AtualizarContato(ref nome, ref email, ref tl);
                        break;

                    //Excluir contato -=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
                    case 4:
                        Console.WriteLine("Excluir um contato");
                        Console.Write("Email: ");
                        emailContato = Console.ReadLine();
                        if (ExcluirContato(ref nome, ref email, ref tl, emailContato))
                        {
                            Console.WriteLine("Contato excluído");
                        }
                        else
                        {
                            Console.WriteLine("Contato não encontrado");
                        }
                        Console.ReadKey();
                        break;

                    //Localizar contato -=-=-=-=-=-=-=--=-=-=-=-==-=-=-=-=-=-=-=-=-=-
                    case 5:
                        Console.WriteLine("Localizar um contato");
                        Console.Write("Email: ");
                        emailContato = Console.ReadLine();
                        pos = LocalizarContato(email, tl, emailContato);
                        if (pos != -1)
                        {
                            Console.WriteLine("Nome: {0} - E-mail: {1}", nome[pos], email[pos]);
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Contato não encontrado");
                        }
                        break;
                }
            }
            //salvar dados no arquivo texto
            BackupAgenda.SalvarDados(ref nome, ref email, ref tl);
        }
    }
}
