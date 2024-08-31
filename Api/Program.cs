using  MininalApi;



IHostBuilder CreateHosBuilder(string[] args){
    return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(WebBuilder => 
    {
        WebBuilder.UseStartup<Startup>();
    });
}

CreateHosBuilder(args).Build().Run();