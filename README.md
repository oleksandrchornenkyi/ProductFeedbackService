## how to run:

- clone the repository
- open folder in terminal
- run dotnet run
- open swagger (url shown in console, usually http://localhost:xxxx/swagger)

## implemented endpoints:

- post /api/feedback -> create feedback, returns id
- get /api/feedback/rating/{productId} -> returns average rating for a product
- post /api/words -> add or update a word/phrase in the dictionary
- get /api/words -> view dictionary
- get /api/wordstats -> view dictionary with averageScore based on all feedbacks
- (optional) get /api/feedback -> list all feedbacks

## logic details:

- words are stored in db, seeded with: excellent=5, good=4, average=3, bad=2, very bad=1
- calculator normalizes text to lowercase, splits into tokens, checks two-word phrases first, then single words, final score is an average
- background job runs every 5 seconds (can be changed in code) and recalculates product ratings and word statistics

## notes:

- database is in memory, all data resets after restart
- unique index on productId in productRatings prevents duplicate entries
- word statistics are simplified: single words may also count when inside a phrase (e.g. "bad" inside "very bad"), this is a known limitation

## example quick test:

- post feedback with productId=1, text = "excellent and good"
- post feedback with productId=1, text = "average performance"
- post feedback with productId=1, text = "bad but good"
- then get /api/feedback/rating/1 -> should be 3.5
- get /api/wordstats -> averageScore for good/excellent/bad will differ from their base score
