control.sh
- checks if db exists
- initializses db
- starts program
- stops program
- inspects db via flag_tool

flag_tool.c (makes sues of SQlite3)
- tags specific entries in "message" table
- reads entries that are tagged with x

minitwit.py
- creates app using Flask
- queries db
- initializes db
- connects to db
- getUserId from user
- format datetime for better displaying
- get avatar (gravater) image from emial
- timeline direcetion
- handles connection to db before  each request
    - "Make sure we are connected to the database each request and look
    up the current user so that we know he's there."
- handles connection after db before  each request
    - "Make sure we are connected to the database each request and look
    up the current user so that we know he's there."
- handles initialization login, logout, follow, unfollow, register, authethication 
