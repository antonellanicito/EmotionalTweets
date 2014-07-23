IMPORTANT NOTE:
Mashape Sentimental API is not more working.

App Description: 
Emotional Tweets allows a user to search recent tweets which will then be displayed showing the emotional context alongside.

  1.  Allow users to enter search terms or hash tags

  2.  Retrieve the tweets using the Twitter search REST API (https://dev.twitter.com/docs/api/1.1/get/search/tweets)

  3.  Using the Sentimental API determine if the tweet is Happy / Sad / Indifferent (https://www.mashape.com/sentimental/sentiment-analysis-for-twitter-and-facebook#!documentation)



Projects inside solution:
1. Emotional Tweets Web: Web Project
2. Emotional Tweets Model: Definition of business objects
3. Emotional Tweets Provider: Interface between Web project and external tools (API in this test project)
I've defined one interface with two methods
GetTweets retrieves list of tweets from typed text
GetHappyness is used to retrieve level of happyness for a tweet (using mashape API), but it wouuld be used to retrieve level of happyness for facebook posts too.
4. Emotional Tweets Utils: static class for common use

Notes.
1. since max number of retrieved tweets from restapi is 15, the view is limited to those ones.
2. Sometimes mashape api doesn't work, mainly reported error is: "not supported language". in this case, it's not possible to determine level of happyness.
3. Since there isn't (maybe I've not found) a mapping between mashape response and level of happyness, I have set a rule to determine level:

After a look to response get from the api test exposed from mashape website, I've noticed that mashape api returns ALWAYS these fields: "value" (possible values: -1.0 < value < -1 ) and "sent" (possible values: 1, -1 or 0) so I've set this rule:
if  (-0.5 <= value <= 0.5) 
{
 level is indifferent
 }
else
{
	if sent = 1 level is happy
	if sent = -1 level is sad
}

4.The input text has not been tested for different typed characters or sentences.

 