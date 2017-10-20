rsync -avz -e 'ssh' bin/Release/netcoreapp2.0/publish/ deploy@web-app.io:/home/deploy/client-tests

ssh deploy@web-app.io  "sudo supervisorctl restart client-tests"
