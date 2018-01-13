namespace Opportunity.MathExpression.Parsing
{
    class Token
    {
        private Token(TokenType type, int position)
        {
            this.Type = type;
            this.Position = position;
        }

        public Token(double number, int position)
            : this(TokenType.Number, position)
        {
            this.Number = number;
        }

        public Token(string id, int position)
            : this(TokenType.Id, position)
        {
            this.Id = id;
        }

        public TokenType Type
        {
            get;
        }

        public string Id
        {
            get;
        }

        public double Number
        {
            get;
        }

        public int Position
        {
            get;
        }

        public override string ToString()
        {
            switch (Type)
            {
            case TokenType.Number:
                return Number.ToString();
            case TokenType.Id:
                return Id;
            case TokenType.LeftBracket:
                return "(";
            case TokenType.RightBracket:
                return ")";
            case TokenType.Plus:
                return "+";
            case TokenType.Minus:
                return "-";
            case TokenType.Multiply:
                return "*";
            case TokenType.Divide:
                return "/";
            case TokenType.Power:
                return "^";
            case TokenType.Comma:
                return ",";
            case TokenType.EOF:
                return "(EoF)";
            default:
                return Type.ToString();
            }
        }

        public static Token Plus(int position) => new Token(TokenType.Plus, position);
        public static Token Minus(int position) => new Token(TokenType.Minus, position);
        public static Token Multiply(int position) => new Token(TokenType.Multiply, position);
        public static Token Divide(int position) => new Token(TokenType.Divide, position);
        public static Token Power(int position) => new Token(TokenType.Power, position);
        public static Token LeftBracket(int position) => new Token(TokenType.LeftBracket, position);
        public static Token RightBracket(int position) => new Token(TokenType.RightBracket, position);
        public static Token Comma(int position) => new Token(TokenType.Comma, position);
        public static Token EOF(int position) => new Token(TokenType.EOF, position);
    }
}
