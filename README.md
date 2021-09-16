# Wishlist
Gift exchange management

![image](https://user-images.githubusercontent.com/56076748/131735933-ec9ef0c6-c29a-4f3d-adc7-6cd3b8ef1633.png)

The goal of this repo is twofold.  
1) to play with Blazor
2) to recreate my family wishlist website on a free platform (Heroku with PostgreSQL)

The original wishlist website (created in classic ASP, then changed over the years) 
allowed multiple families to exchange gifts on an individual level or on a per-family level.
As people have gotten older requirements have changed! There is now more interest in 
a secret santa approach, so it no longer matters who is in what family.  

![image](https://user-images.githubusercontent.com/56076748/112738493-65dbb880-8f31-11eb-9ae3-b9b9d1572067.png)

The need is to be able to:

- Create a list (of items with optional details/web link/cost), 
- View the list of other users,
- Check-off items on other people's list,
- Be unable to see who (if anyone) checked-off items on your list,
- Additionally admin pages should allow user and gift management to some useful degree.
- Forgot password feature (uses Mailgun) which should benefit everyone (especially me),
- Super simple log in (usernames are selected from a dropdown as my family demands convenient/easy access),
- Uploaded photo resize/cropping (using external package), 
- SignalR chat window for users online simulatneously, including read-aloud ability (following a Channel9 demo https://channel9.msdn.com/Shows/On-NET/Using-SignalR-in-your-Blazor-applications).

![image](https://user-images.githubusercontent.com/56076748/131736324-3e424521-930f-4358-8485-14f7c4e5148c.png)

The backlog of potential items...

The option for grouping users (by family or whatever) to create a group list ("board game for the whole gang!") - this could be handled by a normal account but then the sense of belonging to this group needs to be tracked (ala FamilyID or GroupID) to avoid being able to see the items checked-off by others on your associated group list.  This does prevent a member of the group from buying an item "for the group" as it would sort of be buying something for yourself - so this may not fit the needs of all group gift exchanges.  Hence this sits in the backlog as I ponder if it's needed anymore by my secret-santa giving family style...
The ability to secretly "suggest" items on the list of another user without them being able to see the item.  This has been requested by end-users and has potential.  Though a rare case, imagine Joe wants a fishing rod and talks about it, but forgets to put it on his list...you add the rod so that others can buy it or so you can buy it and make sure everyone else sees this is happening (to avoid yet another person getting Joe a fishing rod).
Address all TODOs and HACKS in code.
Unit testing (maybe with Selenium and Bunit?).
Security/code-quality scans.
...and more!

![image](https://user-images.githubusercontent.com/56076748/131736887-eb04e487-fc6d-4446-8da9-4d612bc6225f.png)

===

SETUP - Requires environemnt variables
My current approach is to allow my family user-base to register themselves.
As such I have a registration feature which requires a code, and that code is stored in an environment variable.
Failing to store a code will make it impossible to register users through the UI as the error will tell you no authorization codes exist.
I have one code for regular users and another for super users (capable of editing and impersonating other users).

As defined in the RegisterRequest class...

  //These codes are manually set and sent to users whom we want to register
  Environment.GetEnvironmentVariable("WISHLIST_USER_AUTH_CODE");
  Environment.GetEnvironmentVariable("WISHLIST_ADMIN_AUTH_CODE");

Other environment vars of note...

	WISHLIST_DEVELOPER_EMAIL - the email that is alerted when a new user is created (by an admin) or self-registers.  I do this for awareness and because the free MailGun requires adding each email that can receive messages so I need to know the email of each user joining.  This will work fine as a free solution for my limited (under 30 users) use case.
	WISHLIST_MAILGUN_API_KEY - self explanatory, see heroku.com
	WISHLIST_MAILGUN_DOMAIN - same as above
	
I have PostgreSQL connection in appsettings configured locally (in DEBUG mode) but 
use Heroku's DATABASE_URL environment variable when publishing there (which runs in RELEASE mode).

I'm probably forgetting some things but that's the idea behind my first public GitHub repo.  

Blazor is still quite new and exciting to use.  I look forward to supplying my family another useful website while learning a lot in the process.
