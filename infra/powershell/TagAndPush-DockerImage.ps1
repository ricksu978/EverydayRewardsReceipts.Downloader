param(
    [string]$tarFile,
    [string]$repository,
    [string]$sha,
    [string]$latest,
    [string]$version
)

$shaTag = "${repository}:$sha"
$latestTag = "${repository}:$latest"

# If the tar file exists, load it, give new tag and remove the old tag
if (Test-Path -Path $tarFile) {
    docker load -i $tarFile
    docker tag $sha $shaTag
    docker rmi $sha
}
# If the tar file doesn't exist, pull the image from the repository
else {
    docker pull $shaTag
}

# Tag the image with the version tag
if ($version -match '^v\d+\.\d+\.\d+') {
    $versionTag = "${repository}:$version"
    docker tag $shaTag $versionTag
}

# Tag the image with the latest tag
docker tag $shaTag $latestTag

# Push the image to the repository
docker push $repository --all-tags

