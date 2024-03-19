using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.GameStates
{
    public class GameStateManager
    {
        private ContentManager _content;

        // pojedynczy byt statusu
        private static GameStateManager _instance;

        //stos statusów
        private Stack<GameState> _screens = new Stack<GameState>();

        public static GameStateManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }

        //ustawienie kontent managera
        public void SetContent(ContentManager content)
        {
            _content = content;
        }

        //dodanie nowego ekranu do stosu
        public void AddScreen(GameState screen)
        {
            try
            {
                //dodaj ekran
                _screens.Push(screen);
                //inicjalizuj ekran
                _screens.Peek().Initialize();
                //wywołanie LoadContent na ekran
                if(_content != null)
                {
                    _screens.Peek().LoadContent(_content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add screen\n" + ex.ToString());
            }
        }

        //usunięcie górnego ekranu ze stosu
        public void RemoveScreen()
        {
            if(_screens.Count > 0)
            {
                try
                {
                    var screen = _screens.Peek();
                    _screens.Pop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to delete screen from stack\n" + ex.ToString());
                }
            }
        }

        //wyczyść wszystkie ekrany z listy
        public void ClearScreens()
        {
            while(_screens.Count > 0)
            {
                _screens.Pop();
            }
        }

        //wyczyszcza wszystkie ekrany i daje nowy
        public void ChangeScreen(GameState screen)
        {
            try
            {
                ClearScreens();
                AddScreen(screen);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to change screen\n" + ex.ToString());
            }
        }

        //aktualizacja górnego ekranu
        public void Update(GameTime gameTime)
        {
            try
            {
                if(_screens.Count > 0)
                {
                    _screens.Peek().Update(gameTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to update screen\n" + ex.ToString());
            }
        }

        //rysowanie górnego ekranu
        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if(_screens.Count > 0)
                {
                    _screens.Peek().Draw(spriteBatch);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to draw top screen\n" + ex.ToString());
            }
        }

        //rozładowanie zasobów
        public void UnloadContent()
        {
            foreach(GameState state in _screens)
            {
                state.UnloadContent();
            }
        }
    }
}
