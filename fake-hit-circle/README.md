# Fake Hit Circle

THE FILE WON'T BE RELEASED UNTIL I'M HAPPY WITH IT! (so not yet)

Here are all of the special behaviours that have been implemented into the template script file!
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

4 -> Moves the circle from a selectable edge to a specified position.
Param1 -> Which side should the circles come from?
1 -> Only the left
2 -> Only the right
3 -> Only from above
4 -> Only from below
Any combination of these numbers can be used, for example:
23 -> From both right and above (chosen randomly)
14 -> From both left and below (chosen randomly)
1234 -> From any side (chosen randomly)
The order of each number doesn't matter!
