using System.Text;
using Code.Events;
using Core.GameEvent;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.EJY.UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI enemyCntText;
        [SerializeField] private CanvasGroup skipButtonGroup;

        [Header("Timer Polishing")] [SerializeField]
        private Color warningColor = Color.red;

        [SerializeField] private float warningTime = 10f;
        [SerializeField] private float colorChangeDuration = 0.1f;
        [SerializeField] private float timerShakeDuration = 0.25f;
        [SerializeField] private float timerShakeInterval = 0.25f;
        [SerializeField] private float timerShakePower = 2.5f;

        private StringBuilder _sb;
        private Sequence _seq;
        private int _beforeSec = -1;

        private void Awake()
        {
            _sb = new StringBuilder();
            uiChannel.AddListener<WaveTimerEvent>(HandleWaveTimer);
            uiChannel.AddListener<WaveInfoEvent>(HandleWaveInfo);
            uiChannel.AddListener<ChangeWaveProgress>(HandleChangeWaveProgress);
        }


        private void OnDestroy()
        {
            uiChannel.RemoveListener<WaveTimerEvent>(HandleWaveTimer);
            uiChannel.RemoveListener<WaveInfoEvent>(HandleWaveInfo);
            uiChannel.RemoveListener<ChangeWaveProgress>(HandleChangeWaveProgress);
        }

        private void HandleWaveInfo(WaveInfoEvent evt)
        {
            waveText.text = $"웨이브 {evt.currentWave}";
            enemyCntText.text = $"적 개체 수 : {evt.enemyCnt}";
        }

        private void HandleWaveTimer(WaveTimerEvent evt)
        {
            int min = (int)(evt.timer / 60f);
            int sec = (int)(evt.timer % 60f);

            bool isWarning = min == 0 && sec <= warningTime;

            if (sec == 0 && _seq.IsActive())
            {
                _seq.Kill();
            }

            if (_beforeSec == sec) return;

            _beforeSec = sec;

            if (isWarning && timerText.color != warningColor)
            {
                timerText.DOColor(warningColor, colorChangeDuration);

                if ((_seq == null || !_seq.IsActive()) && sec != 0)
                {
                    _seq = DOTween.Sequence();
                    _seq.Append(timerText.transform.DOShakePosition(timerShakeDuration, timerShakePower, 100))
                        .AppendInterval(timerShakeInterval)
                        .SetLoops(-1);
                }
            }
            else if (!isWarning && timerText.color != Color.white)
            {
                timerText.DOColor(Color.white, colorChangeDuration);

                if (_seq != null && _seq.IsActive())
                {
                    _seq.Kill();
                }
            }

            _sb.Clear();
            _sb.Append("다음 웨이브까지 : ").Append(min.ToString("D2")).Append(":").Append(sec.ToString("D2"));
            timerText.text = _sb.ToString();
        }
        
        private void HandleChangeWaveProgress(ChangeWaveProgress evt)
        {
            float alpha = evt.inProgress ? 0.1f : 1;
            skipButtonGroup.alpha = alpha;
            skipButtonGroup.interactable = !evt.inProgress;
        }
    }
}
