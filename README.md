# FluentT Unity Utilities

FluentT Unity 프로젝트를 위한 애니메이션 및 에셋 유틸리티 도구 모음입니다.

## Tools

모든 도구는 Unity 메뉴 **FluentT > Tools** 에서 접근할 수 있습니다.

### Animation Clip Path Remapper

AnimationClip의 커브 경로(path)를 일괄 재매핑합니다.

- 여러 경로 쌍을 등록하여 한 번에 변환
- **Copy to Folder**: 원본 유지, 새 `.anim` 생성 (FBX/Clip 모두 가능)
- **Apply to Original**: 원본 `.anim` 파일에 직접 적용 (독립 `.anim`만 가능)
- 경로 쌍은 EditorPrefs에 저장되어 유지됨

### Animation Curve Stripper

Blacklist 기반으로 불필요한 애니메이션 커브를 제거합니다.

- **Muscle Blacklist**: prefix 매칭 (시작 문자열 일치 시 제거)
- **BlendShape Blacklist**: exact 매칭 (정확히 일치 시 제거)
- 제거 전 키프레임 수 분석 및 경고 표시
- Blacklist는 편집 가능하며 EditorPrefs에 저장

### Animation Motion Damper

Head/Neck 모션 커브 값을 감쇠시켜 idle 스타일 애니메이션을 생성합니다.

- Head / Neck 별도 감쇠 비율 조절 (0 = 제거, 1 = 원본 유지)
- 적용 전 원본/감쇠 범위 미리보기
- 새 `.anim` 파일로 출력 (원본 유지)

### BlendShape Inspector

선택한 FBX 또는 SkinnedMeshRenderer의 블렌드쉐이프 목록을 확인합니다.

- Scene의 GameObject 선택 시 root 기준 상대경로 표시
- Project의 FBX 에셋 선택 지원
- **Copy to Clipboard** / **Open in Text Editor** (OS 기본 프로그램으로 열기)

### FBX Animation Extractor

FBX 파일에서 AnimationClip을 독립 `.anim` 파일로 추출합니다.

- 파일명: `{FBX이름}_{애니메이션이름}.anim`
- 기존 파일 덮어쓰기 옵션
- 여러 FBX 일괄 처리

## 설치 방법

### Git URL (Unity Package Manager)

1. Unity에서 **Window > Package Manager** 열기
2. 좌측 상단 **+** 버튼 > **Add package from git URL...** 선택
3. 아래 URL 입력:

```
https://github.com/fluentt-ai-repo/fluentT_unity_util.git
```

### Git Submodule

프로젝트의 `Packages/` 폴더에 서브모듈로 추가:

```bash
git submodule add https://github.com/fluentt-ai-repo/fluentT_unity_util.git Packages/FluentTUnityUtil
```

### 요구 사항

- Unity 6000.0 이상
