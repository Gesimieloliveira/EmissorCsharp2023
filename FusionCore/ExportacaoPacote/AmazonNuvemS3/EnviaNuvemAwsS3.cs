using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace FusionCore.ExportacaoPacote.AmazonNuvemS3
{
    public static class EnviaNuvemAwsS3
    {
        //TODO 1612 - Credenciais S3 bucket para pacote xml
        private static readonly IAmazonS3 _amazonS3 = new AmazonS3Client(new BasicAWSCredentials("USER", "PASS"), RegionEndpoint.SAEast1);
        private static readonly string _bucketS3Nome = "bucket-name";
        private static readonly string _urlBase = "https://basename.s3-sa-east-1.amazonaws.com/";

        public static string UploadArquivoZip(string caminhoPacoteXml)
        {
            var caminhoS3 = $"{Guid.NewGuid()}/pacote.zip";

            var utilitarioTransferenciaArquivo =
                new TransferUtility(_amazonS3);

            utilitarioTransferenciaArquivo.Upload(caminhoPacoteXml, _bucketS3Nome, caminhoS3);

            return _urlBase + caminhoS3;
        }
    }
}