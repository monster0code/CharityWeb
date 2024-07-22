// feedback.js

document.addEventListener('DOMContentLoaded', function () {
    // 插入CSS样式
    const style = document.createElement('style');
    style.textContent = `
    .feedback-button-container {
        position: fixed;
        bottom: 20px;
        right: 20px;
        width: 100px;
        height: 40px;
        border: none;
        background-color: #4CAF50;
        color: white;
        box-shadow: 0 0 10px rgba(0,0,0,0.1);
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
        font-size: 14px;
        color: white;
        cursor: pointer;
        border-radius: 5px;
    }
    .feedback-modal {
        display: none;
        position: fixed;
        z-index: 1;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        overflow: auto;
        background-color: rgb(0,0,0);
        background-color: rgba(0,0,0,0.4);
        padding-top: 60px;
    }
    .feedback-modal-content {
        background-color: #fff;
        margin: 5% auto;
        padding: 20px;
        border: 1px solid #888;
        width: 30%;
        box-shadow: 0 0 10px rgba(0,0,0,0.25);
        border-radius: 10px;
        position: relative;
    }
    .feedback-modal-close {
        color: #aaa;
        position: absolute;
        top: 10px;
        right: 10px;
        font-size: 24px;
        font-weight: bold;
    }
    .feedback-modal-close:hover,
    .feedback-modal-close:focus {
        color: black;
        text-decoration: none;
        cursor: pointer;
    }
    .feedback-rating {
        display: flex;
        justify-content: space-between;
        margin-bottom: 20px;
    }
    .feedback-rating input {
        display: none;
    }
    .feedback-rating label {
        font-size: 18px;
        cursor: pointer;
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 5px;
        flex: 1;
        text-align: center;
        margin: 0;
    }
    .feedback-rating input:checked + label {
        background-color: #4CAF50;
        color: white;
        border-color: #4CAF50;
    }
    .feedback-textarea {
        width: calc(100% - 10px);
        padding: 10px;
        margin-bottom: 20px;
        border: 1px solid #ccc;
        border-radius: 5px;
        resize: vertical;
        margin-right: 0;
    }
    .feedback-modal-buttons {
        text-align: right;
    }
    .feedback-modal-buttons button {
        background-color: #4CAF50;
        color: white;
        border: none;
        padding: 10px 20px;
        cursor: pointer;
        border-radius: 5px;
        margin-left: 10px;
    }
    .feedback-modal-buttons button.cancel {
        background-color: #ccc;
        color: black;
    }
    `;
    document.head.appendChild(style);

    // 插入HTML结构
    const feedbackHtml = `
    <div class="feedback-button-container" id="feedbackButton">
        Feedback
    </div>

    <div id="feedback-modal" class="feedback-modal">
        <div class="feedback-modal-content">
            <span class="feedback-modal-close">&times;</span>
            <h2>How would you rate your experience?</h2>
            <div class="feedback-rating">
                <input type="radio" id="rate1" name="rating" value="1"><label for="rate1">1</label>
                <input type="radio" id="rate2" name="rating" value="2"><label for="rate2">2</label>
                <input type="radio" id="rate3" name="rating" value="3"><label for="rate3">3</label>
                <input type="radio" id="rate4" name="rating" value="4"><label for="rate4">4</label>
                <input type="radio" id="rate5" name="rating" value="5"><label for="rate5">5</label>
                <input type="radio" id="rate6" name="rating" value="6"><label for="rate6">6</label>
                <input type="radio" id="rate7" name="rating" value="7"><label for="rate7">7</label>
                <input type="radio" id="rate8" name="rating" value="8"><label for="rate8">8</label>
                <input type="radio" id="rate9" name="rating" value="9"><label for="rate9">9</label>
            </div>
            <textarea class="feedback-textarea" placeholder="How can we improve?" rows="4" id="feedbackText"></textarea>
            <div class="feedback-modal-buttons">
                <button class="cancel">Cancel</button>
                <button class="submit">Submit</button>
            </div>
        </div>
    </div>
    `;
    document.body.insertAdjacentHTML('beforeend', feedbackHtml);

    // JavaScript功能
    var feedbackModal = document.getElementById("feedback-modal");
    var feedbackBtn = document.getElementById("feedbackButton");
    var feedbackSpan = document.getElementsByClassName("feedback-modal-close")[0];

    // 反馈按钮的事件监听器
    var feedbackBtn = document.getElementById("feedbackButton");
    feedbackBtn.addEventListener('click', function () {
        feedbackModal.style.display = "block";
    });

    // 模态框关闭按钮的事件监听器
    feedbackSpan.addEventListener('click', function () {
        closeFeedbackModal();
    });

    // 窗口点击关闭模态框的事件监听器
    window.addEventListener('click', function (event) {
        if (event.target == feedbackModal) {
            closeFeedbackModal();
        }
    });

    function closeFeedbackModal() {
        feedbackModal.style.display = "none";
    }

    // 取消按钮的事件监听器
    var cancelBtn = document.querySelector('.cancel');
    if (cancelBtn) {
        cancelBtn.addEventListener('click', function () {
            closeFeedbackModal();
        });
    }
    var submitBtn = document.querySelector('.submit');
    if (submitBtn) {
        submitBtn.addEventListener('click', function () {
            submitFeedback();
        });
    }
    function submitFeedback() {
        var rating = document.querySelector('input[name="rating"]:checked').value;
        var feedbackText = document.getElementById("feedbackText").value;

        var feedbackData = {
            Rating: parseInt(rating),
            FeedbackText: feedbackText
        };

        console.log(feedbackData);

        // 使用fetch发送AJAX请求到后端
        fetch('/Feedback/SubmitFeedback', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('[name=__RequestVerificationToken]').value
            },
            body: JSON.stringify(feedbackData)
        })
        .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log('Success:', data);
                    closeFeedbackModal();
                } else {
                    console.error('Error:', data.errors);
                }
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
});
