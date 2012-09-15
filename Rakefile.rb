task :deploy do
  sh "cp -rf ./Web/. ../heroku/multi-libs"
  cd "../heroku/multi-libs"
  sh "git status"
  #sh "git commit -m \"deploy\""
  #sh "git push heroku master"
  #cd "..\multi-libs"
end
