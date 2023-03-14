.DEFAULT_GOAL := generate

compose:
	docker compose -f ./docker-compose-monitoring.yml up
run:
	dotnet run --configuration Release -p MiniTwit/Server