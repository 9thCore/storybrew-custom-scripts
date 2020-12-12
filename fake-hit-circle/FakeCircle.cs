using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class FakeCircle : StoryboardObjectGenerator
    {

        //This is a REALLY basic fake circle script. It's a framework, if you will. It works by itself, but what point would there be for a fake circle script for it to act just like a normal circle?
        //Edit this script and stuff to make behaviour different.
        //Please don't redistribute this file under your name.

        [Configurable]
        public int ComboNr = 1; //Combo number, you can change this to any number, it'll work! (hopefully).

        [Configurable]
        public bool RepeatComboNr = false; //If you wanna repeat the first digit of the Combo Number "infinitely", go ahead and set this to true. *It's not actually infinitely, it's just RepeatTimes times.

        [Configurable]
        public int RepeatTimes = 100; //Amount of times to repeat the first digit.

        [Configurable]
        public int ComboColor = 1; //Slight misnomer, it refers to the combo color that should be used. (1 is the first combo color, 2 is the second etc)

        [Configurable]
        public int Time = 0; //The time at which the circle appears

        [Configurable]
        public Vector2 Position = new Vector2(256, 192); //Position, duh

        [Configurable]
        public float ARVALUE = -1; //Set this value to 0 or less in storybrew if you want the script to use the beatmap's AR!

        [Configurable]
        public bool isFake = false; //Set this to true if you want the circle to just fade out instead of "being hit".

        [Configurable]
        public int SpecialBehaviour = 0;
        //Set this to 0 for it to act like a normal fake circle.
        //This template script has a few special behaviours, serving as examples. You can make your own if you need to!
        //Each Special Behaviour can use, by default, 4 floats as parameters. You can edit this file to use more, though.
        //The special behaviour is detailed in the README file in the Github scripts page. https://github.com/9thCore/storybrew-custom-scripts/tree/main/fake-hit-circle

        [Configurable]
        public float Param1 = 0; //The first parameter used by a special behaviour.

        [Configurable]
        public float Param2 = 0; //The second parameter used by a special behaviour.

        [Configurable]
        public float Param3 = 0; //The third parameter used by a special behaviour.

        [Configurable]
        public float Param4 = 0; //The fourth parameter used by a special behaviour.

        public double AR = 0; //Misnomer, is really just the ms representative of the approach rate

        //Gets the AR, in ms. (It's kinda weird so i just did it in a method)
        public void GetTrueAR()
        {
            //I use this variable instead of just using Beatmap.ApproachRate so it's possible for the creator to specify different AR values for each circle! (wow!)
            float usedAR = 0;

            if(ARVALUE>=0) usedAR = ARVALUE; else usedAR = (float)(Beatmap.ApproachRate);

            if(usedAR>=5){
                AR = 450+(10-usedAR)*150;
            }else{
                AR = 1320+(4-usedAR)*120;
            }
        }

        public float Clamp(float value, float min, float max)
        {
            if(value<min){
                value=min;
            }else if(value>max){
                value=max;
            }
            return value;
        }

        //This is where all of the Generate()-ing happens. (get it? the method's called Generate() HAHAHAHHAHAHAHAHAHAAHAHAHAHA)
        //At first I used multiple functions for ApproachCircles, HitCircles etc but I realised that it would be slightly harder to do complex stuff having to jump from one function to another so I just dumped all of the vars in here.
        //This is also where you can edit code to make circles different, if you wanna. (I mean, it would just be a regular circle otherwise, wouldn't it?)
        public override void Generate()
        {

            Position.X = Position.X + 1;

            var CS = Beatmap.CircleSize;

            var xpos = Random(0, 640);
            var ypos = Random(0, 480);

            //This is where (most) of the special behaviours are checked.
            //I saw most because one might want to change the approach circle location, for example. At that point, they'd do the check at the approach circle.
            if(SpecialBehaviour==1){
                ARVALUE = Random(Math.Min(Param1, 12.5f), Math.Min(Param2, 12.5f));
            }else if(SpecialBehaviour==2){
                CS = Random(Math.Min(Math.Max(0, Param1),10), Math.Min(Math.Max(0, Param2),10));
            }

            //Working around the fact that you can't choose the last combo colour, for some reason..
            var comboColors = Beatmap.ComboColors.ToList();
            comboColors.Add(Beatmap.ComboColors.Last());
            comboColors.Add(Beatmap.ComboColors.Last());

            ComboColor = (int)Clamp(ComboColor, 1, Beatmap.ComboColors.Count());
            
            ARVALUE = Clamp(ARVALUE, -1, 12.5f); //I clamp it to 12.5 because coding

            //Convert the playfield area coords to storyboard coords (so you can just place an object, copy it's coords and it'll work)
            Position.X = Position.X + 60f;
            Position.Y = Position.Y + 55f;
            //fun fact, i spent like 4 hours doing complex algorithms and this was all i had to do ._.

            GetTrueAR();

            var FadeIn = 400f;
            
            if(ARVALUE>10){
                FadeIn = 400f - (ARVALUE-10)*150f;
            }

            //just layer
		    var layer = GetLayer("Main");

            //This is the hitcircle thing
            var HitCircle = layer.CreateSprite("hitcircle.png", OsbOrigin.Centre, Position);
            HitCircle.Color(Time - AR, Time + 250, comboColors.ElementAt(ComboColor+1), comboColors.ElementAt(ComboColor+1));

            //This is the hitcircleoverlay thing
            var HitCircleOverlay = layer.CreateSprite("hitcircleoverlay.png", OsbOrigin.Centre, Position);

            var len = ComboNr.ToString().Length;

            //Number stuff
            //Transform ComboNr to actual in-game number

            //Also, if you want it to just repeat the first digit over and over, set RepeatComboNr to true in the customizable fields.
            if(!RepeatComboNr){
                for(int i=0;i<len;i++){
                    var size = (109.0f - 9.0f*CS)/120.0f;
                    var number = layer.CreateSprite("default-" + ComboNr.ToString()[i] + ".png", OsbOrigin.Centre, Position);

                    var factor = 20f;

                    if(CS>=8&&CS<9){
                        factor = 15f;
                    }else if(CS>=9&&CS<10){
                        factor = 10f;
                    }else if(CS==10){
                        factor = 5f;
                    }

                    var xPos = Position.X + i*factor;
                    var newXPos = xPos - (len-1)*factor/2;

                    if(SpecialBehaviour==4){
                
                        Param1 = Clamp(Param1, 1, 4);

                        if(Param1==1){
                            xpos = 0;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }else if(Param1==2){
                            xpos = 640;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }else if(Param1==3){
                            ypos = 0;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }else if(Param1==4){
                            ypos = 480;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }
                    }else{
                        number.MoveX(Time, Time, newXPos, newXPos);
                    }

                    number.Scale(Time - AR, Time, size*0.75f, size*0.75f);
                    number.Fade(Time - AR, Time - AR + FadeIn, 0, 1);

                    if(SpecialBehaviour==5){
                        number.Fade(Time - AR + FadeIn + 1, Time - AR + FadeIn + AR/4, 1, 0);
                    }else{
                        number.Fade(Time, Time + 60, 1, 0);
                    }
                }
            }else{
                for(int i=0;i<RepeatTimes-1;i++){
                    var size = (109.0f - 9.0f*CS)/120.0f;
                    var number = layer.CreateSprite("default-" + ComboNr.ToString()[0] + ".png", OsbOrigin.Centre, Position);

                    var xPos = Position.X + i*20f;
                    var newXPos = xPos - (RepeatTimes-2)*10f;

                    if(SpecialBehaviour==4){
                
                        Param1 = Clamp(Param1, 1, 4);

                        if(Param1==1){
                            xpos = 0;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }else if(Param1==2){
                            xpos = 640;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }else if(Param1==3){
                            ypos = 0;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }else if(Param1==4){
                            ypos = 480;
                            number.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), new Vector2(newXPos, Position.Y));
                        }
                    }else{
                        number.MoveX(Time, Time, newXPos, newXPos);
                    }

                    number.Scale(Time - AR, Time, size*0.75f, size*0.75f);
                    number.Fade(Time - AR, Time - AR + FadeIn, 0, 1);

                    if(SpecialBehaviour==5){
                        number.Fade(Time - AR + FadeIn + 1, Time - AR + FadeIn + AR/4, 1, 0);
                    }else{
                        number.Fade(Time, Time + 60, 1, 0);
                    }
                }
            }

            var xOffsetL = 0f;
            var yOffsetU = 0f;
            var xOffsetR = 0f;
            var yOffsetD = 0f;

            if(SpecialBehaviour==3){
                xOffsetL = Param1;
                yOffsetU = Param2;
                xOffsetR = Param3;
                yOffsetD = Param4;
            }

            //This is the approach circle thing
            var ApproachCircle = layer.CreateSprite("approachcircle.png", OsbOrigin.Centre, new Vector2(Position.X + Random(-xOffsetL, xOffsetR), Position.Y + Random(-yOffsetU, yOffsetD)));
            ApproachCircle.Color(Time - AR, Time, comboColors.ElementAt(ComboColor+1), comboColors.ElementAt(ComboColor+1));

            HitCircle.Scale(Time - AR, Time, (109.0f - 9.0f*CS)/120.0f, (109.0f - 9.0f*CS)/120.0f);
            HitCircle.Fade(Time - AR, Time - AR + FadeIn, 0, 1);

            HitCircleOverlay.Scale(Time - AR, Time, (109.0f - 9.0f*CS)/120.0f, (109.0f - 9.0f*CS)/120.0f);
            HitCircleOverlay.Fade(Time - AR, Time - AR + FadeIn, 0, 1);            

            ApproachCircle.Scale(Time - AR, Time, (109.0f - 9.0f*CS)/120.0f * 3.75, (109.0f - 9.0f*CS)/120.0f);

            if(SpecialBehaviour!=5){

                ApproachCircle.Fade(Time - AR, Time - AR + FadeIn, 0, 1);
                HitCircleOverlay.Fade(Time, Time + 250, 1, 0);
                HitCircle.Fade(Time, Time + 250, 1, 0);
                if(!isFake){
                    HitCircleOverlay.Scale(OsbEasing.Out, Time, Time + 250, (109.0f - 9.0f*CS)/120.0f, (109.0f - 9.0f*CS)/120.0f*1.25);
                    HitCircle.Scale(OsbEasing.Out, Time, Time + 250, (109.0f - 9.0f*CS)/120.0f, (109.0f - 9.0f*CS)/120.0f*1.25);
                }
            }else{
                HitCircleOverlay.Fade(Time - AR + FadeIn, Time - AR + FadeIn + AR/4, 1, 0);
                //No approach circle with HD!
                ApproachCircle.Fade(Time - AR, Time - AR, 0, 0);
                HitCircle.Fade(Time - AR + FadeIn, Time - AR + FadeIn + AR/4, 1, 0);
            }

            if(SpecialBehaviour==4){
                
                Param1 = Clamp(Param1, 1, 4);

                if(Param1==1){
                    //left
                    xpos = 0;
                    HitCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    HitCircleOverlay.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    ApproachCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                }else if(Param1==2){
                    //right
                    xpos = 640;
                    HitCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    HitCircleOverlay.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    ApproachCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                }else if(Param1==3){
                    //above
                    ypos = 0;
                    HitCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    HitCircleOverlay.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    ApproachCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                }else if(Param1==4){
                    //below
                    ypos = 480;
                    HitCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    HitCircleOverlay.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                    ApproachCircle.Move(OsbEasing.Out, Time - AR, Time - 100f, new Vector2(xpos, ypos), Position);
                }
            }
        }
    }
}
