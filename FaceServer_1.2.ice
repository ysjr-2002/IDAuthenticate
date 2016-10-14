// **********************************************************************
//
// Copyright (c) 2003-2016 ZeroC, Inc. All rights reserved.
//
// **********************************************************************

#pragma once

module FaceRecognitionModule
{

    //字节数组定义
    sequence<byte> ByteSeq;

    //标签列表定义
    sequence<string> TagList;

    //人脸识别内容
    struct FaceInfo{
    	int x;
    	int y;
    	int width;
    	int height;  
    	ByteSeq imgInfo;  //图片字节流
    };

    // 用户信息
    struct PersonInfo{
        string id;     //数据记录唯一标识
        string uuid;   //业务唯一标识
        string code;   //编号
        string name;   //姓名
        string race;    //种族, 预留
        string nationality;  //国籍, 预留
        string gender; //性别, 预留
        ByteSeq img1;
        ByteSeq img2;
        ByteSeq img3;
        ByteSeq signatureCode1;
        ByteSeq signatureCode2;
        ByteSeq signatureCode3;
        TagList tagList;
    };


    //数组定义
    sequence<PersonInfo> FaceInfoSeq;
    //人脸识别结果
    struct FaceInfoResult{
        FaceInfoSeq faceInfoList;
        int code; // 成功:200; 其他参考常量表
        string message;   // 如code非0，则加上附加信息
    };

    //1:1人脸识别结果定义
    struct CompareResult{
    	float similarity; //总体相似度
    	int code; // 成功:200; 其他参考常量表
        string message;   // 如code非0，则加上附加信息
    };

    //1:n人脸识别对象
    struct VerifySignatureCodeInfo{
    	float similarity; //总体相似度
    	PersonInfo personInfo; // 人物信息
    };

    //1:n人脸识别对象数组定义
    sequence<VerifySignatureCodeInfo> VerifySignatureCodeInfoList;
    struct VerifySignatureCodeResult{
        VerifySignatureCodeInfoList verifySignatureCodeInfoList;
        int totalCount; // 总记录数
    };

    struct OperationResult{ 
        int code; // 成功:200; 其他参考常量表
        string message;   // 如code非0，则加上附加信息
    };

    sequence<PersonInfo> PersonInfoList;
    struct QueryPersonResult{
        int totalCount; // 总记录数
        PersonInfoList personInfoList; // 结果集
    };


    // ============== callback interface ==============
    interface ClientCallbackReceiver
    {
        void detectCallback(FaceInfoSeq faceInfoSeq);
    };

    // ============== server interface ==============
    interface FaceRecognition
    {
        //人脸捕捉
        //params img : 图片字节
        //params threshold 比对最低要求阀值，结果需大于或等于该阀值的
        //params maxImageCount : 每次识别所返回图片的最多人脸数
        idempotent FaceInfoResult staticDetect(ByteSeq img, float threshold, int maxImageCount);

        //1:1人脸匹配
        //params srcImg : 待比较图片的字节
        //params destImg : 目标图片的字节
        idempotent CompareResult compare(ByteSeq srcImg, ByteSeq destImg);

        //获取特征码： 
        //params img : 图片字节
        idempotent ByteSeq convertSignatureCode(ByteSeq img);

        //rtsp人脸捕捉
        //params rtspPath : 摄像枪rtsp数据流
        //params proxy : 回调函数
        //params threshold 比对最低要求阀值，结果需大于或等于该阀值的
        //params maxImageCount : 每次识别所返回图片的最多人脸数
        //params interval  : 间隔时间(单位：毫秒)
        //回调接口
        idempotent OperationResult dynamicDetect(string rtspPath, ClientCallbackReceiver* proxy, float threshold, int maxImageCount, long interval);

        // 停止动态人脸捕捉
        OperationResult shutdownDynamicDetect();

    	//根据特征码查询所匹配的人脸信息，按匹配度降序排列, 1:n
        //params signatureCode 人脸特征码
        //params threshold 比对最低要求阀值，结果需大于或等于该阀值的
        //params offset 下标
        //params size      返回的匹配个数
        idempotent VerifySignatureCodeResult verifySignatureCode(ByteSeq signatureCode, float threshold, int offset, int size);
        
        //模板库更新操作
    	//params uuid;         // 人物唯一ID
        //params name;         // 人物名称
        //params code;         // 人物证件编号
    	//params img1 img2 img3;          //人脸图片文件
    	//params signatureCode1 signatureCode2 signatureCode3;  //特征码
        idempotent OperationResult createOrUpdatePerson(string uuid, string name, string code, ByteSeq img1, ByteSeq signatureCode1, ByteSeq img2, ByteSeq signatureCode2, ByteSeq img3, ByteSeq signatureCode3);

        // 更新人物标签
        idempotent OperationResult updatePersonTags(string uuid, TagList tagList);

        // 删除人物指定标签
        //params uuid;         // 人物唯一ID
        //params tagList  指定删除的标签，如为 null 则删除对应人物的所有标签
        idempotent OperationResult deletePersonTags(string uuid, TagList tagList);

        //人物库删除
    	//params uuid;         // 人物唯一ID
        idempotent OperationResult removePerson(string uuid);

        //人物库查询 
        //params id             //记录唯一标识
        //params uuid;         // 人物唯一ID
    	//params code;         //特征码
        //params tagList;       // 标签列表
        //params offset      下标
        //prams size         每页记录数
        idempotent QueryPersonResult queryPerson(string id, string uuid, string code, TagList tagList, int offset, int size);


    };

};
