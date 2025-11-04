using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpaceType
{
    EMPTY,
    WALL 
}

public class SpyBoard
{
    private string name;
    private int width;
    private int height;
    private SpaceType[,] board;
    private List<BoardUnit> units = new();

    Vector2Int startPos;
    Vector2Int endPos;
    
    public SpyBoard(TextAsset _boardFile)
    {
        Debug.Log("Attempting to parse " +  _boardFile.name);
        ParseBoard(_boardFile.text);
    }

    /* 
     * Parses a text file to generate board
     * 
     * Text file should be in format:
     * (Replace tokens in <> with corresponding values
     * 
     * "<NAME>"
     * 
     * <WIDTH> 
     * <HEIGHT>
     * 
     * <BOARD>
     * 
     * <UNITS>
     */
    public void ParseBoard(string _text)
    {
        // Parse Functions ---------------------------------------------------

        // Matches and consumes a token string from start of text
        void MatchToken(string _token)
        {
            if (_text.StartsWith(_token)) _text = _text[_token.Length..];
            else throw new FormatException("FAILED TO PARSE TOKEN " + _token);
        }

        // Consumes and returns char at start of text
        char ConsumeChar()
        {
            if (_text.Length == 0) throw new FormatException("CANNOT PARSE CHAR FROM EMPTY STRING");
            char c = _text[0];
            _text = _text[1..];
            return c;
        }

        // Consumes and returns string ending in ending char from text (not including the ending character)
        string ConsumeString(char _end)
        {
            int _index = _text.IndexOf(_end);
            if (_index != -1)
            {
                string _str = _text[.._index];
                _text = _text[(_index + 1)..];
                return _str;
            }
            else throw new FormatException("FAILED TO PARSE STRING ENDING IN [" + _end + "]");
        }

        // Consumes and returns string up to newline character
        string ConsumeLine()
        {
            int _index = _text.IndexOf('\n');
            string _str;
            if (_index != -1)
            {
                _str = _text[..(_index-1)];
                _text = _index < _text.Length ? _text[(_index+1)..] : "";
            }
            else
            {
                _str = _text;
                _text = "";
            }
            return _str;
        }

        // Consumes all spaces and control characters at start of text
        void ConsumeEmpty()
        {
            if (_text.Length == 0) return;

            int index = 0;
            while (char.IsWhiteSpace(_text[index]) || char.IsControl(_text[index]))
            {
                index++;
                if (index == _text.Length)
                {
                    _text = "";
                    return;
                }
            }
            
            _text = _text[index..];
        }

        // Consumes and returns integer at start of text ending in ending char
        int ConsumeInt(char _end)
        {
            int _index = _text.IndexOf(_end);
            if (_index != -1)
            {
                if (int.TryParse(_text[.._index], out int num))
                {
                    _text = _text[(_index + 1)..];
                    return num;
                }
                else
                {
                    throw new FormatException("FAILED TO PARSE INT ENDING IN [" + _end + "], INVALID STRING");
                }
            }
            else throw new FormatException("FAILED TO PARSE INT ENDING IN [" + _end + "]");
        }

        // Returns type of space given token char
        SpaceType ParseSpace(char _token, int i, int j)
        {
            switch (_token)
            {
                case 'S':
                    startPos = new Vector2Int(i, j);
                    return SpaceType.EMPTY;

                case 'E':
                    endPos = new Vector2Int(i, j);
                    return SpaceType.EMPTY;

                case '-':
                    return SpaceType.EMPTY;

                case '#':
                    return SpaceType.WALL;

                default:
                    throw new FormatException("CANNOT PARSE SPACE TYPE FROM TOKEN [" + _token + "]");
            };
        }

        // Parses and consumes board
        void ConsumeBoard()
        {
            board = new SpaceType[height,width];
            for (int i = 0; i < height; i++)
            {
                string _boardLine = ConsumeLine();
                if (_boardLine == "") throw new FormatException("COULD NOT FIND MAP LINE");
                if (_boardLine.Length != width) throw new FormatException("BOARD LINE NOT CORRECT WIDTH");

                for (int j = 0; j < width; j++)
                {
                    board[i,j] = ParseSpace(_boardLine[j], i, j);
                }
            }
        }

        // Consumes and returns a coordinate at start of text
        Vector2Int ConsumeCoordinate()
        {
            MatchToken("(");
            int x = ConsumeInt(',');
            int y = ConsumeInt(')');
            return new Vector2Int(x, y);    
        }

        // Consumes and returns a list of type T from text using a function which consumes that type
        List<T> ConsumeList<T>(Func<T> _elementConsumer)
        {
            MatchToken("(");
            List<T> list = new();
            while (_text[0] != ')')  list.Append(_elementConsumer());
            MatchToken(")");
            return list;
        }

        // Consumes and construsts a guard unit from text
        void ConsumeGuard()
        {
            List<Vector2Int> patrolPath = ConsumeList(ConsumeCoordinate);
            MatchToken(",");
            char direction = ConsumeChar();
            MatchToken(",");
            int range = ConsumeInt(')');
            // TODO: Create guard
            Debug.Log("Parsed Guard");
        }

        // Consumes and constructs board unit from text
        void ConsumeUnit()
        {
            string unit = ConsumeString('(');
            switch (unit)
            {
                case "Guard":
                    ConsumeGuard();
                    break;

                default:
                    throw new FormatException("CANNOT PARSE UNIT " + unit);
            }
            ConsumeEmpty();
        }

       // -------------------------------------------------------------------
        
        try
        {
            // Parse name
            MatchToken("\"");
            name = ConsumeString('\"');
            ConsumeEmpty();
            Debug.Log("Parsed name: " + name);

            // Parse map width and height
            width = ConsumeInt('\n');
            height = ConsumeInt('\n');
            ConsumeEmpty();
            Debug.Log("Parsed width: " + width + " Parsed height: " + height);

            // Parse board
            ConsumeBoard();
            
            ConsumeEmpty();

            // Parse units
            while (_text.Length > 0) ConsumeUnit();

            Debug.Log("Parse successful");

        }
        catch (FormatException e)
        {
            Debug.LogError("PARSE ERROR: " + e.Message);
        }
    }

    // Determines if playerPos is a valid move based on the board space
    public bool EvaluatePosition(Vector2Int playerPos)
    {
        return board[playerPos[0], playerPos[1]] switch
        {
            SpaceType.EMPTY => true,
            SpaceType.WALL => false,
            _ => true
        };
    }

    // Determines if playerPos is a valid move based on board units
    public bool EvaluateUnits(Vector2Int playerPos)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (!units[i].Update(playerPos, board)) return false;
            
        }
        return true;
    }

    // Determines if path through board is valid (assumes path is connected)
    public bool EvaluatePath(List<Vector2Int> path)
    {
        if (path.Count < 2) return false;
        if (path[0] != startPos || path[^1] != endPos) return false;

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int playerPos = path[i];
            if (!EvaluatePosition(playerPos) || !EvaluateUnits(playerPos)) return false;
        }

        return true;
    }
}
