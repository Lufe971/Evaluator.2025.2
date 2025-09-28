namespace Evaluator.Core;

public class ExpressionEvaluator
{
    public static double Evaluate(string infix)
    {
        var tokens = Tokenize(infix);
        var postfix = InfixToPostfix(tokens);
        return Calculate(postfix);
    }

    // Convierte string en lista de tokens (números y operadores)
    private static List<string> Tokenize(string infix)
    {
        var tokens = new List<string>();
        var number = string.Empty;

        foreach (char c in infix.Replace(" ", "")) // quitar espacios
        {
            if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else if (IsOperator(c))
            {
                if (!string.IsNullOrEmpty(number))
                {
                    tokens.Add(number);
                    number = string.Empty;
                }
                tokens.Add(c.ToString());
            }
            else
            {
                throw new Exception($"Carácter inválido en la expresión: {c}");
            }
        }

        if (!string.IsNullOrEmpty(number))
        {
            tokens.Add(number);
        }

        return tokens;
    }

    // Convierte infix a postfix usando Shunting Yard
    private static List<string> InfixToPostfix(List<string> infixTokens)
    {
        var stack = new Stack<string>();
        var postfix = new List<string>();

        foreach (var token in infixTokens)
        {
            if (double.TryParse(token, out _))
            {
                postfix.Add(token);
            }
            else if (IsOperator(token[0]))
            {
                if (token == ")")
                {
                    while (stack.Peek() != "(")
                        postfix.Add(stack.Pop());
                    stack.Pop(); // eliminar "("
                }
                else
                {
                    while (stack.Count > 0 && PriorityInfix(token[0]) <= PriorityStack(stack.Peek()[0]))
                        postfix.Add(stack.Pop());

                    stack.Push(token);
                }
            }
        }

        while (stack.Count > 0)
            postfix.Add(stack.Pop());

        return postfix;
    }

    // Calcula resultado desde postfix
    private static double Calculate(List<string> postfix)
    {
        var stack = new Stack<double>();

        foreach (var token in postfix)
        {
            if (double.TryParse(token, out double number))
            {
                stack.Push(number);
            }
            else if (IsOperator(token[0]))
            {
                var op2 = stack.Pop();
                var op1 = stack.Pop();
                stack.Push(Calculate(op1, token[0], op2));
            }
        }

        return stack.Pop();
    }

    private static bool IsOperator(char item) => item is '^' or '/' or '*' or '%' or '+' or '-' or '(' or ')';

    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' or '/' or '%' => 2,
        '+' or '-' => 1,
        '(' => 5,
        _ => throw new Exception("Operador inválido."),
    };

    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' or '/' or '%' => 2,
        '+' or '-' => 1,
        '(' => 0,
        _ => throw new Exception("Operador inválido."),
    };

    private static double Calculate(double op1, char op, double op2) => op switch
    {
        '*' => op1 * op2,
        '/' => op1 / op2,
        '^' => Math.Pow(op1, op2),
        '+' => op1 + op2,
        '-' => op1 - op2,
        '%' => op1 % op2,
        _ => throw new Exception("Operador inválido."),
    };
}
