$composeFiles = @(
    "compose.api.yaml",
    "compose.db.yaml",
    "compose.cache.yaml",
    "compose.identity.yaml",
    "compose.rabbitmq.yaml",
    "compose.helpers.yaml"
)

$composeCommand = "docker compose"
foreach ($file in $composeFiles) {
    $composeCommand += " -f $file"
}

$composeCommand += " up --abort-on-container-exit"

Invoke-Expression -Command $composeCommand