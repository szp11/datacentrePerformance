var pdf = require('html-pdf');

module.exports = result => {

    var html = "<h1>Hello from Node.js!</h1>";

    pdf.create(html).toStream(function(err, stream)
    {
        //stream.pipe(fs.createWriteStream('./foo.pdf'));
        stream.pipe(result.stream);
    });

    //callback(null,"Node.js -> [" + new Date() + "]");
}