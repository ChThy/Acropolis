import { resolve } from 'path';
import { exec } from 'child_process';

const subPath = process.argv[2] || '';
const swaggerFile = process.argv[3] || '';
const swaggerUrl = new URL(swaggerFile);

if (swaggerUrl.hostname.toLowerCase() === 'localhost') {
  swaggerUrl.hostname = 'host.docker.internal';
  console.log('localhost overwritten with host.docker.internal: ' + swaggerUrl);
}

const dirName = import.meta.dirname;
const projectDir = resolve(dirName, '../src/clients', subPath);
const configDir = resolve(dirName, '../src/clients');

console.log(projectDir);
console.log(configDir);

const command = `docker run --rm -v "${projectDir}:/local" -v "${configDir}:/config" openapitools/openapi-generator-cli generate -i ${swaggerUrl} -g typescript-axios -o /local --config ./config/config.json`;

exec(command, (error, stdout, stderr) => {
  if (error) {
    console.error(`Error: ${error.message}`);
    return;
  }
  if (stderr) {
    console.error(`Stderr: ${stderr}`);
    return;
  }
  console.log(`Stdout: ${stdout}`);
});