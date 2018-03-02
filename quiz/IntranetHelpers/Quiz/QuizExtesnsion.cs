using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Intranet.Models.QuizModel;

namespace Intranet.Controllers
{
    //static class QuizExtesnsion
    //{
    //    public static Quiz GetPictures(this Quiz executionQuiz)
    //    {
    //        //var picrureList = new List<QuestionPictures>();
    //        //foreach (var item in executionQuiz.Questions ?? new List<Question>())
    //        //{
    //        //    var pictures = item.QuestionPictures.Select(x => x.QuestionPicture).ToList();

    //        //    foreach (var pic in pictures)
    //        //    {
    //        //        // var bytes = File.ReadAllBytes(pic);
    //        //        //скачать файл по линку и сохранить его в base64
    //        //        var client = new WebClient();
    //        //        var m_Bytes = client.DownloadData(pic);

    //        //        picrureList.Add(new QuestionPictures
    //        //        {
    //        //            QuestionPicture = Convert.ToBase64String(m_Bytes)
    //        //        });
    //        //    }

    //        //    // picrureList.AddRange(pictures.Select(pic => new QuestionPictures { QuestionPicture = pic.Split(new char[]{';'},2)[1] }));

    //        //    item.QuestionPictures = picrureList;
    //        //}
    //        return executionQuiz;
    //    }

    //}


}