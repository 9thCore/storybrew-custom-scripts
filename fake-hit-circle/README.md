# Fake Hit Circle

This script is, well, a fake hit circle script! (for storybrew, obviously)

To install it, put it in the scripts folder in your storybrew folder. You should be able to use the script in any storyboard after that!

This script also has a few special behaviours, just to get your gears movin' when it comes to innovative gameplay/special behaviours if you want to code more yourself.
Here are all of the special behaviours that have been implemented into the script file!
Also, when there's something which randomizes something, don't forget to set the Random Seed.

1 -> Randomizes the AR.  
Param1 -> The lower bound of the AR.  
Param2 -> The higher bound of the AR. (cannot be higher than 10)  

2 -> Randomizes the CS.  
Param1 -> The lower bound of the CS. (cannot be lower than 0)  
Param2 -> The higher bound of the CS. (cannot be higher than 10)  

3 -> Randomizes an offset for the Approach Circle from the actual circle.  
Param1 -> Horizontal distance, in pixels, from the note, left.  
Param2 -> Vertical distance, in pixels, from the note, up.  
Param3 -> Horizontal distance, in pixels, from the note, right.  
Param4 -> Vertical distance, in pixels, from the note, down.  

4 -> Moves the circle from a selectable edge to a specified position. (the X and Y values)<br/>
Param1 -> Which side should the circles come from?<br/>
1 -> Only from the left<br/>
2 -> Only from the right<br/>
3 -> Only from above<br/>
4 -> Only from below<br/>

5 -> Apply HD mod  
Applies a replica of the HD mod on the circle.
