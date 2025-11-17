# Formster

## To run the project localy please follow these steps

1. Open ``./Fomster.Server/Formster.sln`` in Rider, Visual Studio or another IDE or you can run `dotnet run` from `Formster.Server/Formster.Presentation` folder
2. Create `.env` file in `Formster.Client` folder and enter api url like in `.env.example`
3. Open terminal from `Formster.Client` folder and run `npm install` to install all necessary dependencies
4. Run `npm run dev` from the same folder and open url in browser

## To run the project on docker follow these steps
Run this command in terminal from `Formster` folder

```bash
docker-compose up -d
```

## To build JS and CSS into one file follow these steps
Run this command from `Formster.Client` folder

```bash
npm run build
```
