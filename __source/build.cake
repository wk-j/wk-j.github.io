
Task("Generate").Does(() => {
    StartProcess("hexo", new ProcessSettings {
        Arguments = "generate"
    });
});

Task("Server").Does(() => {
    StartProcess("hexo", new ProcessSettings {
        Arguments = "server"
    });
});


var target = Argument("Target", "Generate");
RunTarget(target);