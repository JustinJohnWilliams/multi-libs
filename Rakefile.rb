task :copy do
  sh "cp -rf ./Web/. ../heroku/multi-libs"
end
