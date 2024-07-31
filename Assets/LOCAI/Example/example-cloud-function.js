const {GoogleGenerativeAI} = require("@google/generative-ai");

exports.LOCAIGemini = functions
    .https.onRequest(async (request, response) => {
      const apiKey = "YOUR API KEY";
      const genAI = new GoogleGenerativeAI(apiKey);
      corsHandler(request, response, async () => {
        const model = genAI.getGenerativeModel({
          model: textModel,
        });
        const myPrompt = request.query.MyPrompt.toString();
        const aiResult = await model.generateContent(myPrompt);
        const aiResponse = await aiResult.response;
        response.send(aiResponse.text());
      });
    });