using System.ComponentModel.Design;
using trabalhoAed;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args)
    {
        Baralho baralho = new Baralho();
        Stack<Carta> monteDeCompra;
        List<Carta> areaDeDescarte;
        Carta cartaDaVez;
        int opcao = 1;
        Jogador[] jogadores;


        Console.WriteLine("BEM VINDO AO JOGO ROUBA MONTES");
        Console.WriteLine("Digite o número de jogadores: ");
        int n = int.Parse(Console.ReadLine());
        jogadores = new Jogador[n];

        CadastroJogadores(jogadores);

        Console.WriteLine("-------------------");

        int tamanho = 0;

        while (tamanho > 208 || tamanho <= 0)
        {
            Console.WriteLine("Digite quantas cartas terá o baralho inicial: **O MÁXIMO DE CARTAS NO NOSSO JOGO É 208**");
            tamanho = int.Parse(Console.ReadLine());

            if (tamanho > 208 || tamanho <= 0)
            {
                Console.WriteLine("Digite um valor maior que 0 ou menor ou igual que 208");
            }
            else
            {
                Console.WriteLine("Número válido!!!");
            }
        }

        Console.WriteLine("-------------------");

        monteDeCompra = baralho.BaralhoInicial(tamanho);
        areaDeDescarte = new List<Carta>();

        Console.Clear();

        Console.WriteLine($"Jogadores Cadastrados:");
        foreach (Jogador jogador in jogadores)
        {
            Console.WriteLine(jogador.Nome);
        }
        Console.WriteLine("Quantidade de Cartas do Monte de Compras: " + monteDeCompra.Count());

        while (monteDeCompra.Count() > 0)
        {
            for (int i = 0; i < jogadores.Length && monteDeCompra.Count > 0; i++)
            {
                Console.WriteLine($"Sua vez {jogadores[i].Nome}");

                MovimentacoesDaPartida(null, monteDeCompra, jogadores[i], jogadores, areaDeDescarte);
               
            }
        }

        Console.WriteLine();
        CalculaPlacar(jogadores);
        Console.WriteLine("Quantidade de cartas da area de descarte " + areaDeDescarte.Count);
        Console.WriteLine("Quantidade de cartas do monte de compra " + monteDeCompra.Count);


    }

    public static void CadastroJogadores(Jogador[] jogadores)
    {
        int n = 1;
        for (int i = 0; i < jogadores.Length; i++)
        {
            Console.WriteLine($"Digite o nome do jogador número {n}: ");
            string nome = Console.ReadLine();
            jogadores[i] = new Jogador(nome);
        }
    }

    public static bool PegaDaAreaDeDescarte(List<Carta> descarte, Carta cartaDaVez, Jogador jogador)
    {
        bool verificacao = false;

        for (int i = 0; i < descarte.Count; i++)
        {
            Carta carta = descarte[i];
            if (carta.Numero == cartaDaVez.Numero)
            {
                jogador.MonteDoJogador.Push(carta);
                descarte.Remove(carta);
                verificacao = true;
            }
            else
            {
                verificacao = false;
            }
        }
        return verificacao;
    }

    public static void CalculaPlacar(Jogador[] jogadores)
    {
        for (int i = 0; i < jogadores.Length; i++)
        {
            for (int j = 0; j < jogadores.Length; j++)
            {
                if (jogadores[j].MonteDoJogador.Count < jogadores[i].MonteDoJogador.Count)
                {
                    Jogador temp = jogadores[j];
                    jogadores[j] = jogadores[i];
                    jogadores[i] = temp;
                }
            }
        }

        Console.WriteLine("QUADRO DE POSIÇÃO");
        Console.WriteLine("-----------------");
        for (int i = 0; i < jogadores.Length; i++)
        {
            Console.WriteLine($" {i + 1}º: {jogadores[i].Nome} com {jogadores[i].MonteDoJogador.Count} cartas");
        }
        Console.WriteLine("-----------------");
    }

    public static void MovimentacoesDaPartida(Carta cartaDaVez, Stack<Carta> monteDeCompra, Jogador jogador, Jogador[] jogadores, List<Carta> areaDeDescarte)
    {
        if (monteDeCompra.Count > 0)
        {
            cartaDaVez = monteDeCompra.Pop();
        }
        Console.WriteLine($"A carta da vez é: {cartaDaVez.Numero}");
        Console.WriteLine($"Tamanho atual do monte: {monteDeCompra.Count()}");

        foreach (Jogador outroJogador in jogadores)
        {
            if (jogador != outroJogador
                && outroJogador.MonteDoJogador.Count > 0
                && cartaDaVez.Numero == outroJogador.MonteDoJogador.Peek().Numero)
            {
                jogador.roubaOutroJogador(outroJogador, cartaDaVez);

                MovimentacoesDaPartida(cartaDaVez, monteDeCompra, jogador, jogadores, areaDeDescarte);
                break;
            }
            else if (PegaDaAreaDeDescarte(areaDeDescarte, cartaDaVez, jogador) == true)
            {
                Console.WriteLine($"{jogador.Nome} pegou da área de descarte");

                MovimentacoesDaPartida(cartaDaVez, monteDeCompra, jogador, jogadores, areaDeDescarte);
                break;
            }
            else if (jogador.MonteDoJogador.Count > 0 && cartaDaVez.Numero == jogador.MonteDoJogador.Peek().Numero)
            {
                jogador.MonteDoJogador.Push(cartaDaVez);
                Console.WriteLine($"Carta da vez foi para o monte de {jogador.Nome}");
                break;
            }
            else
            {
                areaDeDescarte.Add(cartaDaVez);
                Console.WriteLine($"{cartaDaVez.Numero} de {cartaDaVez.Naipe} foi para a área de descarte");
                break;
            }
        }
    }

}