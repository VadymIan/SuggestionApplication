pipeline {
    environment {
    imagename = "mylocaldocker3103/test-project"
    dockerImage = ''
  }

    agent any

    stages {
        stage('Cloning Git') {
            steps {
                git 'https://github.com/VadymIan/BlazorApp.git'
            }
        }
        stage('Building') {
            steps {
		 sh 'dir'
                 sh 'dotnet build BlazorApp1.sln --configuration Release'
            }
        }
        stage('Running Tests') {
            steps {
                sh 'dotnet test --logger "trx;LogFileName=TestResult.trx" TestProject/TestProject.csproj'
            }
            
            post {
                always {
                    ws('/var/lib/jenkins/workspace/BlazorApp/TestProject/TestResults')
                    {
                        mstest()
                    }                    
                }
            }
        }
	stage('Building Image') {
		steps {
			script {
				dockerImage = docker.build(imagename)
			}
		}
	}
	stage('Pushing Image') {
		steps {
			script {
				docker.withRegistry('https://registry.hub.docker.com', 'myapp') {
					dockerImage.push('latest')
				}
			}
		}
	}
    }
}
