//curl -i -H "Content-Type: application/json" -H "Accept: application/json" -X POST -d '{"Name":"bob"}' http://localhost:3000/add

var express = require('express');
var app = express()
var server = require('http').createServer(app);
var people = [
  { Name: "Person 1" },
  { Name: "Person 2" }
];

server.listen(process.env.PORT || 3000);

app.set('view engine', 'ejs');
app.set('view options', { layout: false });
app.use(express.methodOverride());
app.use(express.bodyParser());  
app.use(app.router);

app.get('/list', function (req, res) {
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify(people));
  res.end();
});

app.post('/add', function (req, res) {
  people.push(req.body);
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify(req.body));
  res.end();
});
