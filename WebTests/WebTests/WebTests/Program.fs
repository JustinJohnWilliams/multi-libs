// Learn more about F# at http://fsharp.net

open canopy
open runner

let switchTo b = browser <- b
let index = "http://dry-peak-5299.herokuapp.com/"
let createGame = ".nav li a"
let cardCzar = "#notificationCardCzar"
let waiting = "#notificationWaitingOnRound"
let join = "#availableGames div a"
let selectACard = "#notificationSelectCard"
let select = ".select-card"
let nextRound = "#buttonNextRound a"
let points = "#pointsValue"

let playFirstCard _ =
    selectACard == "select a card to play"
    click select
    select *= "selected"

start firefox
let player1 = browser
start firefox
let player2 = browser
start firefox
let player3 = browser
start firefox
let player4 = browser

tile [player1; player2; player3; player4]

test (fun _ ->
    describe "create a new game and wait"
    switchTo player1
    url index
    click createGame    
    waiting == "waiting on round to start")

test (fun _ ->
    describe "join existing game and wait"
    switchTo player2
    url index
    click join
    waiting == "waiting on round to start")

test (fun _ ->
    describe "join existing game and wait"
    switchTo player3
    url index
    click join
    waiting == "waiting on round to start")

test (fun _ ->
    describe "join existing game as last player and play"
    switchTo player4
    url index
    click join)

test(fun _ ->
    describe "player 2 picks a card"
    switchTo player2
    playFirstCard ())

test(fun _ ->
    describe "player 3 picks a card"
    switchTo player3
    playFirstCard ())

test(fun _ ->
    describe "player 4 picks a card"
    switchTo player4
    playFirstCard ())

test(fun _ ->
    describe "player 1 is czar and chooses card"
    switchTo player1
    click select
    describe "player 1 starts next round"
    click nextRound)

test(fun _ ->
    switchTo player2
    describe "player 2 wins"
    points == "1"
    describe "player 2 starts next round"
    click nextRound)

test(fun _ ->
    switchTo player3
    describe "player 3 loses"
    points == "0"
    describe "player 3 starts next round"
    click nextRound)

test(fun _ ->
    switchTo player4
    describe "player 4 loses"
    points == "0"
    describe "player 4 starts next round"
    click nextRound)

test(fun _ ->
    switchTo player2
    describe "player 2 is the czar"
    cardCzar == "you are the card czar")

run ()

System.Console.ReadLine();

quit ()