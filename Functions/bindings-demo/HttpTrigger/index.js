module.exports = async function (context, req, inputDocument) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (!inputDocument)
    {
        let message= "ToDo item " + req.query.id + " not found";
        context.log(message);

        context.res = {
            status: 404,
            body: message
        };
    }
    else
    {
        context.log("Found ToDo item, Description=" + inputDocument.desc);
        context.res = {
            body: inputDocument.desc
        };
    }
}