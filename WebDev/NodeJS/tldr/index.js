const express = require('express');
const app = express();
const articles = [{ title: 'Example' }];
const bodyParser = require('body-parser');
const Article = require('./db').Article;

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.get('/articles', (req, res, next) => {
    Article.all((err, articles) => {
        if (err) return next(err);
        res.send(articles);
    });
});

app.post('/articles', (req, res, next) => {
    res.send("OK");
});

app.get('/articles/:id', (req, res, next) => {
    const id = req.params.id;
    console.log("Fetching: " + id);
    Article.find(id, (err, article) => {
       if (err) return next(err);
       res.send(article); 
    });
});

app.delete('/articles/:id', (req, res, next) => {
    const id = req.params.id;
    console.log("Deleting: " + id);
    Article.delete(id, (err) => {
       if (err) return ext(err);
       res.send({ message: 'Deleted' });
    });
});

const port = 3000;
app.listen(port);
console.log("Running at http://localhost:%s", port);
module.exports = app;