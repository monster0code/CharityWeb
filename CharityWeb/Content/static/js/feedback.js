// feedback.js
var feedbackModal = document.getElementById("feedback-modal");
var feedbackBtn = document.getElementById("feedbackButton");
var feedbackSpan = document.getElementsByClassName("feedback-modal-close")[0];

feedbackBtn.onclick = function () {
    feedbackModal.style.display = "block";
}

feedbackSpan.onclick = function () {
    feedbackModal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == feedbackModal) {
        feedbackModal.style.display = "none";
    }
}

function closeFeedbackModal() {
    feedbackModal.style.display = "none";
}

function submitFeedback() {
    var rating = document.querySelector('input[name="rating"]:checked').value;
    var feedbackText = document.getElementById("feedbackText").value;

    // 假设这里有一个获取当前用户信息的函数
    var currentUser = getCurrentUser();

    var feedbackData = {
        user: currentUser,
        rating: rating,
        feedback: feedbackText
    };

    // 使用fetch发送AJAX请求到后端
    fetch('/api/feedback', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(feedbackData)
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
            closeFeedbackModal();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function getCurrentUser() {
    // 这是一个示例函数，返回一个假定的当前用户对象
    // 实际使用中，您需要替换成获取当前登录用户信息的代码
    return {
        id: 12345,
        name: "John Doe",
        email: "john.doe@example.com"
    };
}
